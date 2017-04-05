using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CrearObjetos.DTO.Utilerias
{
    public class Response<T> : IDisposable
    {
        [DefaultValue(StatusType.Ok)]
        public StatusType StatusType { get; set; }
        public List<String> ListError { get; set; }
        public string UserMessage { get; set; }
        public Int32 RecordsCount { get; set; }
        public Int32 IdEntity { get; set; }
        public List<T> ListRecords { get; set; }
        public Int32 StatusResponse { get; set; }
        public T ResponseType { get; set; }
        private bool disposing;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool b)
        {
            if (!disposing)
            {
                disposing = true;
                GC.SuppressFinalize(this);
            }
        }

        ~Response()
        {
            Dispose(true);
        }
    }
}
