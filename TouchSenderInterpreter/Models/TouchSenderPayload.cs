namespace TouchSenderInterpreter.Models
{
    public record TouchSenderPayload(
            int Id,
            DeviceInfo DeviceInfo,
            SingleTouch? SingleTouch = null
        );

    public record DeviceInfo(
            int Width,
            int Height
        );

    public record SingleTouch(
            double X,
            double Y
        );
}
