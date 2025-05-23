﻿using System.Text.Json;

using TouchSenderInterpreter.Models;

namespace TouchSenderInterpreter
{
    public class Interpreter
    {

        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
#if NET9_0_OR_GREATER
            RespectNullableAnnotations = true,
            RespectRequiredConstructorParameters = true,
#endif
        };

        public static TouchSenderResult Read(byte[] input)
        {
            try
            {
                return new TouchSenderResult(Payload: JsonSerializer.Deserialize<TouchSenderPayload>(input, _options), IsSuccess: true);
            }
            catch (JsonException ex)
            {
                return new TouchSenderResult(IsSuccess: false, ErrorMessage: ex.Message);
            }
        }
    }
}
