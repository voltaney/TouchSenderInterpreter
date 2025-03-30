namespace TouchSenderInterpreter.Models
{
    public record TouchSenderPayload(
            int Id,
            DeviceInfo DeviceInfo,
            SingleTouch? SingleTouch = null
        )
    {
        public SingleTouch? SingleTouchRatio
        {
            get
            {
                // If SingleTouch is null, return null
                if (SingleTouch == null) return null;
                return new SingleTouch(
                    X: SingleTouch.X / DeviceInfo.Width,
                    Y: SingleTouch.Y / DeviceInfo.Height
                );
            }
        }
    };

    public record DeviceInfo(
            int Width,
            int Height
        );

    public record SingleTouch(
            double X,
            double Y
        );
}
