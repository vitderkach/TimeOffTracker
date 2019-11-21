using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TOT.Utility.Validation;

namespace TOT.Utility.ViewHelpers
{
    public static class ModelStateHelper
    {
        public static string SerialiseModelState(ModelStateDictionary modelState)
        {
            var errorList = modelState
                .Select(ms => new ModelStateTransferData
                {
                    Key = ms.Key,
                    AttemptedValue = ms.Value.AttemptedValue,
                    RawValue = ms.Value.RawValue,
                    ErrorMessages = ms.Value.Errors.Select(err => err.ErrorMessage).ToList(),
                });

             return JsonConvert.SerializeObject(errorList);
        }

        public static ModelStateDictionary DeserialiseModelState(string serialisedErrorList)
        {
            var errorList = JsonConvert.DeserializeObject<List<ModelStateTransferData>>(serialisedErrorList);
            var modelState = new ModelStateDictionary();

            foreach (var item in errorList)
            {
                modelState.SetModelValue(item.Key, item.RawValue, item.AttemptedValue);
                foreach (var error in item.ErrorMessages)
                {
                    modelState.AddModelError(item.Key, error);
                }
            }
            return modelState;
        }
    }
}
