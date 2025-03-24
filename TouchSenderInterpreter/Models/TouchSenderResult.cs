namespace TouchSenderInterpreter.Models
{
    public record TouchSenderResult(
            TouchSenderPayload? Payload = null,
            bool IsSuccess = false,
            string? ErrorMessage = null
        );
}
