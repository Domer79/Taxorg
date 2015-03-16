using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace SystemTools.WebTools.Providers
{
    public class SessionStateProvider :  SessionStateStoreProviderBase
    {
        private string _connectionString = ApplicationCustomizer.ConnectionString;
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
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param>
        /// <param name="id">Объект <see cref="P:System.Web.SessionState.HttpSessionState.SessionID"/> для текущего запроса.</param>
        /// <param name="locked">При возврате этим методом, содержит значение логического типа, равное true, если запрашиваемый элемент сеанса заблокирован в хранилище данных сеанса, или значение false в противном случае.</param><param name="lockAge">При возврате этим методом содержит объект <see cref="T:System.TimeSpan"/> со значением, равным количеству времени, в течение которого элемент в хранилище данных сеанса оставался заблокированным.</param>
        /// <param name="lockId">При возврате этим методом содержит объект, значение которого равно идентификатору блокировки для текущего запроса. Дополнительные данные об идентификаторе блокировки см. в разделе "Блокирование данных в хранилище сеанса" в кратком обзоре класса <see cref="T:System.Web.SessionState.SessionStateStoreProviderBase"/>.</param>
        /// <param name="actions">При возврате этим методом содержит одно из значений <see cref="T:System.Web.SessionState.SessionStateActions"/>, позволяющее определить, является ли текущий сеанс неинициализированным сеансом без поддержки файлов Cookie.</param>
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
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/> для текущего запроса.</param>
        /// <param name="id">Объект <see cref="P:System.Web.SessionState.HttpSessionState.SessionID"/> для текущего запроса.</param>
        /// <param name="locked">При возврате этим методом содержит логическое значение, равное true в случае успешного получения блокировки, или значение false в противном случае.</param><param name="lockAge">При возврате этим методом содержит объект <see cref="T:System.TimeSpan"/> со значением, равным количеству времени, в течение которого элемент в хранилище данных сеанса оставался заблокированным.</param><param name="lockId">При возврате этим методом содержит объект, значение которого равно идентификатору блокировки для текущего запроса. Дополнительные данные об идентификаторе блокировки см. в разделе "Блокирование данных в хранилище сеанса" в кратком обзоре класса <see cref="T:System.Web.SessionState.SessionStateStoreProviderBase"/>.</param>
        /// <param name="actions">При возврате этим методом содержит одно из значений <see cref="T:System.Web.SessionState.SessionStateActions"/>, позволяющее определить, является ли текущий сеанс неинициализированным сеансом без поддержки файлов Cookie.</param>
        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked,
            out TimeSpan lockAge,
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
}
