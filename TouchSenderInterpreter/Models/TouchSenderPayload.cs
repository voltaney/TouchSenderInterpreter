namespace TouchSenderInterpreter.Models
{
    public record TouchSenderPayload(
            int Id,
            Deviceinfo DeviceInfo,
            Singletouch SingleTouch
        )
    {
        public Singletouch SingleTouchRatio
        {
            get
            {
                return new Singletouch(
                    X: SingleTouch.X / DeviceInfo.Width,
                    Y: SingleTouch.Y / DeviceInfo.Height
                );
            }
        }
    };

    public record Deviceinfo(
            int Width,
            int Height
        );

    public record Singletouch(
            double? X,
            double? Y
        );
}
