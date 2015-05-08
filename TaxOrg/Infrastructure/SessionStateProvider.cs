using System;
using System.Collections.Specialized;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;
using TaxorgRepository.Models;
/* Взято с http://habrahabr.ru/post/141056/ */
using WebSecurity;

namespace TaxOrg.Infrastructure
{
    /// <summary>
    /// Наш собственный провайдер.
    /// </summary>
    public class SessionStateProvider : SessionStateStoreProviderBase
    {
        TaxorgContext _dataContext;
        int _timeout;

        /// <summary>
        /// Инициализация провайдера, читаем конфигурацию, устаналиваем переменные...
        /// </summary>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null) throw new ArgumentNullException("config");
            base.Initialize(name, config);

            var applicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            var configuration = WebConfigurationManager.OpenWebConfiguration(applicationName);

            var configSection = (SessionStateSection)configuration.GetSection("system.web/sessionState");
            _timeout = (int)configSection.Timeout.TotalMinutes;

            // Контекст, который мы получили с помощью EntityFramework для удобной работы с базой данных.
            // Здесь можно использовать Dependency Injection для создания объекта и передачи строки подключения.

            _dataContext = new TaxorgContext();
        }

        public override void Dispose()
        {
            _dataContext.Dispose();
        }

        /// <summary>
        /// Получаем сессию для режима "только для чтения" без необходимости блокировки.
        /// </summary>
        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return GetSessionItem(context, id, false, out locked, out lockAge, out lockId, out actions);
        }

        /// <summary>
        /// Получаем сессию в режиме эксклюзивного доступа с необходимостью блокировки.
        /// </summary>
        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return GetSessionItem(context, id, true, out locked, out lockAge, out lockId, out actions);
        }

        /// <summary>
        /// Обобщенный вспомогательный метод для получения доступа к сессии в базе данных.
        /// Используется как GetItem, так и GetItemExclusive.
        /// </summary>
        private SessionStateStoreData GetSessionItem(HttpContext context, string id, bool exclusive, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            locked = false;
            lockAge = new TimeSpan();
            lockId = null;
            actions = 0;

            var sessionItem = _dataContext.Sessions.Find(id);

            // Сессия не найдена
            if (sessionItem == null) return null;

            // Сессия найдена, но заблокирована
            if (sessionItem.Locked)
            {
                locked = true;
                lockAge = DateTime.UtcNow - sessionItem.LockDate;
                lockId = sessionItem.LockId;
                return null;
            }

            // Сессия найдена, но она истекла
            if (DateTime.UtcNow > sessionItem.Expires)
            {
                _dataContext.Entry(sessionItem).State = EntityState.Deleted;
                _dataContext.SaveChanges();
                return null;
            }

            // Сессия найдена, требуется эксклюзинвый доступ.
            if (exclusive)
            {
                sessionItem.LockId += 1;
                sessionItem.Locked = true;
                sessionItem.LockDate = DateTime.UtcNow;
                _dataContext.SaveChanges();
            }

            locked = exclusive;
            lockAge = DateTime.UtcNow - sessionItem.LockDate;
            lockId = sessionItem.LockId;

            var data = (sessionItem.ItemContent == null)
                ? CreateNewStoreData(context, _timeout)
                : Deserialize(context, sessionItem.ItemContent, _timeout);

            data.Items["UserId"] = sessionItem.UserId;

            return data;
        }

        /// <summary>
        /// Удаляем блокировку сессии, освобождаем ее для других потоков.
        /// </summary>
        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            var sessionItem = _dataContext.Sessions.Find(id);
            if (sessionItem.LockId != (int)lockId) return;

            sessionItem.Locked = false;
            sessionItem.Expires = DateTime.UtcNow.AddMinutes(_timeout);
            _dataContext.SaveChanges();
        }

        /// <summary>
        /// Сохраняем состояние сессии и снимаем блокировку.
        /// </summary>
        public override void SetAndReleaseItemExclusive(HttpContext context,
                                                        string id,
                                                        SessionStateStoreData item,
                                                        object lockId,
                                                        bool newItem)
        {
            var intLockId = lockId == null ? 0 : (int)lockId;
            var userId = (int)item.Items["UserId"];

            var data = ((SessionStateItemCollection)item.Items);
            data.Remove("UserId");

            // Сериализуем переменные
            var itemContent = Serialize(data);

            // Если это новая сессия, которой еще нет в базе данных.
            if (newItem)
            {
                var session = new Session
                {
                    SessionId = id,
                    UserId = userId,
                    Created = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(_timeout),
                    LockDate = DateTime.UtcNow,
                    Locked = false,
                    ItemContent = itemContent,
                    LockId = 0,
                };

                _dataContext.Sessions.Add(session);
                _dataContext.SaveChanges();
                return;
            }

            // Если это старая сессия, проверяем совпадает ли ключ блокировки, 
            // а после сохраняем состояние и снимаем блокировку.
            var state = _dataContext.Sessions.Find(id);
            if (state.LockId == (int)lockId)
            {
                state.UserId = userId;
                state.ItemContent = itemContent;
                state.Expires = DateTime.UtcNow.AddMinutes(_timeout);
                state.Locked = false;
                _dataContext.SaveChanges();
            }
        }

        /// <summary>
        /// Удаляет запись о состоянии сессии.
        /// </summary>
        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            var state = _dataContext.Sessions.Find(id);
            if (state.LockId != (int)lockId) return;

            _dataContext.Entry(state).State = EntityState.Deleted;
            _dataContext.SaveChanges();
        }

        /// <summary>
        /// Сбрасывает счетчик жизни сессии.
        /// </summary>
        public override void ResetItemTimeout(HttpContext context, string id)
        {
            var sessionItem = _dataContext.Sessions.Find(id);
            if (sessionItem == null) return;

            sessionItem.Expires = DateTime.UtcNow.AddMinutes(_timeout);
            _dataContext.SaveChanges();
        }

        /// <summary>
        /// Создается новый объект, который будет использоваться для хранения состояния сессии в течении запроса.
        /// Мы можем установить в него некоторые предопределенные значения, которые нам понадобятся.
        /// </summary>
        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            var data = new SessionStateStoreData(new SessionStateItemCollection(),
                                                    SessionStateUtility.GetSessionStaticObjects(context),
                                                    timeout);

            data.Items["UserId"] = ((UserIdentity)Security.Instance.Principal.Identity).User.IdUser;
            return data;
        }

        /// <summary>
        /// Создание пустой записи о новой сессии в хранилище сессий.
        /// </summary>
        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            var session = new Session
            {
                SessionId = id,
                UserId = 1,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(timeout),
                LockDate = DateTime.UtcNow,
                Locked = false,
                ItemContent = null,
                LockId = 0,
            };

            _dataContext.Sessions.Add(session);
            _dataContext.SaveChanges();
        }

        #region Ненужые методы в данной реализации

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback) { return false; }
        public override void EndRequest(HttpContext context) { }
        public override void InitializeRequest(HttpContext context) { }

        #endregion

        #region Вспомогательные методы сериализации и десериализации

        private byte[] Serialize(SessionStateItemCollection items)
        {
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);

            if (items != null) items.Serialize(writer);
            writer.Close();

            return ms.ToArray();
        }

        private SessionStateStoreData Deserialize(HttpContext context, Byte[] serializedItems, int timeout)
        {
            var ms = new MemoryStream(serializedItems);

            var sessionItems = new SessionStateItemCollection();

            if (ms.Length > 0)
            {
                var reader = new BinaryReader(ms);
                sessionItems = SessionStateItemCollection.Deserialize(reader);
            }

            return new SessionStateStoreData(sessionItems, SessionStateUtility.GetSessionStaticObjects(context), timeout);
        }

        #endregion
    }

}