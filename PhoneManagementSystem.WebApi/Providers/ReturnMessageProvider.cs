namespace PhoneManagementSystem.WebApi.Providers
{
    using System;

    public static class ReturnMessageProvider
    {
        public static object SuccessEdit(object id, string modelName)
        {
            return new ReturnMessage()
            {
                Message = string.Format("Successfully edited {0} #{1}.", modelName, id)
            };
        }

        public static object SuccessDelete(object id, string modelName)
        {
            return new ReturnMessage()
            {
                Message = string.Format("Successfully deleted {0} #{1}.", modelName, id)
            };
        }

        public static object ErrorDublicate(object id, string modelName)
        {
            return new ReturnMessage()
            {
                Message = string.Format("Duplicate {0} #{1}.", modelName, id)
            };
        }
        public static object ErrorMessage(string message)
        {
            return new ReturnMessage()
            {
                Message = message
            };
        }

        private class ReturnMessage
        {
            public string Message { get; set; }
        }
    }
}