using Camed.SSC.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Camed.SSC.WebAPI.Filter
{
    public class ApplicationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Exception domainException = context.Exception as ApplicationException;

            if(domainException != null) {
                context.Result = new JsonResult(new Result {
                    Message = domainException.Message,
                });

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            } else {

                domainException = context.Exception;

                if(domainException.Message.Contains("Nullable object must have a value.")) {
                    context.Result = new JsonResult(new Result {
                        Message = "Dados incomplentos ou não preenchidos. Verique os campos e tente novamente.",
                        Successfully = false
                    });
                } else {
                    context.Result = new JsonResult(new Result {
                        Message = "Um erro interno ocorreu no sistema ao tentar processar sua requisição. Favor, tentar novamente.",
                        Successfully = false
                    });
                }

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
