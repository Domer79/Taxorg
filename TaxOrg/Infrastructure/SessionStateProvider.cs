using System;
using System.Collections.Specialized;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;
using TaxorgRepository.Models;
using WebSecurity;

namespace TaxOrg.Infrastructure
{
    /* Взято с http://habrahabr.ru/post/141056/ */

    /// <summary>
    /// Наш собственный провайдер.
    /// </summary>
    public class SessionStateProvider : SessionStateStoreProviderBase
    {
        /// <summary>
        /// Освобождает все ресурсы, используемые реализацией <see cref="T:System.Web.SessionState.SessionStateStoreProviderBase"/>.
        /// </summary>
        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Задает ссылку на делегата <see cref="T:System.Web.SessionState.SessionStateItemExpireCallback"/> для события Session_OnEnd, заданного в файле Global.asax.
        /// </summary>
        /// <returns>
        /// Если поставщик хранилищ состояния сеанса поддерживает вызов события Session_OnEnd, значение true, если нет, значение false.
        /// </returns>
        /// <param name="expireCallback">Делегат <see cref="T:System.Web.SessionState.SessionStateItemExpireCallback"/> для события Session_OnEnd, заданного в файле Global.asax.</param>
        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Вызывается объектом <see cref="T:System.Web.SessionState.SessionStateModule"/> для инициализации по запросу.
        /// </summary>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param>
        public override void InitializeRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает доступные только для чтения данные о состоянии сеанса из хранилища данных сеанса.
        /// </summary>
        /// <returns>
        /// Объект <see cref="T:System.Web.SessionState.SessionStateStoreData"/>, заполненный значениями сеанса и данными из хранилища данных сеанса.
        /// </returns>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param><param name="id">Объект <see cref="P:System.Web.SessionState.HttpSessionState.SessionID"/> для текущего запроса.</param><param name="locked">При возврате этим методом, содержит значение логического типа, равное true, если запрашиваемый элемент сеанса заблокирован в хранилище данных сеанса, или значение false в противном случае.</param><param name="lockAge">При возврате этим методом содержит объект <see cref="T:System.TimeSpan"/> со значением, равным количеству времени, в течение которого элемент в хранилище данных сеанса оставался заблокированным.</param><param name="lockId">При возврате этим методом содержит объект, значение которого равно идентификатору блокировки для текущего запроса. Дополнительные данные об идентификаторе блокировки см. в разделе "Блокирование данных в хранилище сеанса" в кратком обзоре класса <see cref="T:System.Web.SessionState.SessionStateStoreProviderBase"/>.</param><param name="actions">При возврате этим методом содержит одно из значений <see cref="T:System.Web.SessionState.SessionStateActions"/>, позволяющее определить, является ли текущий сеанс неинициализированным сеансом без поддержки файлов Cookie.</param>
        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId,
            out SessionStateActions actions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает доступные только для чтения данные о состоянии сеанса из хранилища данных сеанса.
        /// </summary>
        /// <returns>
        /// Объект <see cref="T:System.Web.SessionState.SessionStateStoreData"/>, заполненный значениями сеанса и данными из хранилища данных сеанса.
        /// </returns>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param><param name="id">Объект <see cref="P:System.Web.SessionState.HttpSessionState.SessionID"/> для текущего запроса.</param><param name="locked">При возврате этим методом содержит логическое значение, равное true в случае успешного получения блокировки, или значение false в противном случае.</param><param name="lockAge">При возврате этим методом содержит объект <see cref="T:System.TimeSpan"/> со значением, равным количеству времени, в течение которого элемент в хранилище данных сеанса оставался заблокированным.</param><param name="lockId">При возврате этим методом содержит объект, значение которого равно идентификатору блокировки для текущего запроса. Дополнительные данные об идентификаторе блокировки см. в разделе "Блокирование данных в хранилище сеанса" в кратком обзоре класса <see cref="T:System.Web.SessionState.SessionStateStoreProviderBase"/>.</param><param name="actions">При возврате этим методом содержит одно из значений <see cref="T:System.Web.SessionState.SessionStateActions"/>, позволяющее определить, является ли текущий сеанс неинициализированным сеансом без поддержки файлов Cookie.</param>
        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge,
            out object lockId, out SessionStateActions actions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Снимает блокировку элемента в хранилище данных сеанса.
        /// </summary>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param><param name="id">Идентификатор сеанса для текущего запроса.</param><param name="lockId">Идентификатор блокировки для текущего запроса.</param>
        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновляет данные об элементе сеанса в хранилище данных состояния сеанса значениями из текущего запроса, а также снимает блокировку данных.
        /// </summary>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param><param name="id">Идентификатор сеанса для текущего запроса.</param><param name="item">Объект <see cref="T:System.Web.SessionState.SessionStateStoreData"/>, содержащий текущие значения сеанса для сохранения.</param><param name="lockId">Идентификатор блокировки для текущего запроса.</param><param name="newItem">Значение true, чтобы обозначить элемент сеанса как новый; значение false чтобы обозначить элемент сеанса как существующий.</param>
        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удаляет данные об элементе из хранилища данных сеанса.
        /// </summary>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param><param name="id">Идентификатор сеанса для текущего запроса.</param><param name="lockId">Идентификатор блокировки для текущего запроса.</param><param name="item">Объект <see cref="T:System.Web.SessionState.SessionStateStoreData"/>, представляющий элемент, который необходимо удалить из хранилища данных.</param>
        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновляет дату и время истечения срока действия элемента в хранилище данных сеанса.
        /// </summary>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param><param name="id">Идентификатор сеанса для текущего запроса.</param>
        public override void ResetItemTimeout(HttpContext context, string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создает новый объект <see cref="T:System.Web.SessionState.SessionStateStoreData"/>, который необходимо использовать для текущего запроса.
        /// </summary>
        /// <returns>
        /// Новый объект <see cref="T:System.Web.SessionState.SessionStateStoreData"/> для текущего запроса.
        /// </returns>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param><param name="timeout">Значение <see cref="P:System.Web.SessionState.HttpSessionState.Timeout"/> для состояния сеанса, заданное для нового объекта <see cref="T:System.Web.SessionState.SessionStateStoreData"/>.</param>
        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавляет новый элемент состояния сеанса в хранилище данных.
        /// </summary>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param><param name="id">Объект <see cref="P:System.Web.SessionState.HttpSessionState.SessionID"/> для текущего запроса.</param><param name="timeout">Свойство <see cref="P:System.Web.SessionState.HttpSessionState.Timeout"/> сеанса для текущего запроса.</param>
        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Вызывается объектом <see cref="T:System.Web.SessionState.SessionStateModule"/> в конце запроса.
        /// </summary>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param>
        public override void EndRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }

/*    public class SessionStateProvider : SessionStateStoreProviderBase
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
            _counter++;
            locked = false;
            lockAge = new TimeSpan();
            lockId = null;
            actions = 0;

            var sessionItem = GetSessionItemFromDb(id);

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

        private static readonly object LockObject = new object();
        private static bool _getSessionItemFromDbIsLocked;
        private static int _counter;

        private Session GetSessionItemFromDb(string id)
        {
            while (_getSessionItemFromDbIsLocked)
            {
                Console.WriteLine("GetSessionFromDb is locked");
            }

            lock (LockObject)
            {
                _getSessionItemFromDbIsLocked = true;
                var result = _dataContext.Sessions.Find(id);
                _getSessionItemFromDbIsLocked = false;

                return result;
            }
        }

        /// <summary>
        /// Удаляем блокировку сессии, освобождаем ее для других потоков.
        /// </summary>
        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            var sessionItem = GetSessionItemFromDb(id);
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
            var state = GetSessionItemFromDb(id);
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
            var state = GetSessionItemFromDb(id);
            if (state.LockId != (int)lockId) return;

            _dataContext.Entry(state).State = EntityState.Deleted;
            _dataContext.SaveChanges();
        }

        /// <summary>
        /// Сбрасывает счетчик жизни сессии.
        /// </summary>
        public override void ResetItemTimeout(HttpContext context, string id)
        {
            var sessionItem = GetSessionItemFromDb(id);
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
*/
}