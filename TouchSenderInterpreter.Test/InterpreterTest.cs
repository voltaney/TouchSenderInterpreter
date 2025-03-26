using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using TouchSenderInterpreter.Models;

using Xunit.Abstractions;

namespace TouchSenderInterpreter.Test
{
    internal class TestDataGenerator
    {
        static readonly int ExampleId = 10;
        static readonly DeviceInfo FullDeviceInfo = new(Width: 1920, Height: 1080);
        static readonly SingleTouch FullSingleTouch = new(X: 0.5, Y: 0.5);
        static readonly TouchSenderPayload FullPayload = new(ExampleId, FullDeviceInfo, FullSingleTouch);
        public static IEnumerable<object[]> FullPayloads()
        {
            yield return new object[]
            {
                new TouchSenderPayload(ExampleId, FullDeviceInfo, FullSingleTouch)
            };
        }

        // Generate payloads with null values
        public static IEnumerable<object[]> PayloadsWithSingleTouchNullX()
        {
            yield return new object[]
            {
                FullPayload with { SingleTouch = FullSingleTouch with { X=null } }
            };
            yield return new object[]
            {
                FullPayload with { SingleTouch = FullSingleTouch with { X=null, Y=null } }
            };
        }

        // Generate payloads with different values
        public static IEnumerable<object[]> DifferenctPayloads()
        {
            var payload1 = new TouchSenderPayload(ExampleId, FullDeviceInfo, FullSingleTouch);
            yield return new object[]
            {
                payload1,
                payload1 with {
                    SingleTouch = FullSingleTouch with { X = null }
                }
            };
        }

        // Generate payloads with same values
        public static IEnumerable<object[]> SamePayloads()
        {
            var payload1 = new TouchSenderPayload(ExampleId, FullDeviceInfo, FullSingleTouch);
            // same reference
            yield return new object[] { payload1, payload1 };
            // same value
            yield return new object[] { payload1, payload1 with { } };
        }
    }
    // Inject ITestOutputHelper to write logs
    public class InterpreterTest(ITestOutputHelper output)
    {
        [Theory]
        [MemberData(nameof(TestDataGenerator.FullPayloads), MemberType = typeof(TestDataGenerator))]
        public void Read_ValidFullJson_ReturnsSuccessResult(TouchSenderPayload payload)
        {
            // Log
            output.WriteLine(JsonSerializer.Serialize(payload));

            // Arrange
            var json = JsonSerializer.Serialize(payload);
            var input = Encoding.UTF8.GetBytes(json);

            // Act
            var result = Interpreter.Read(input);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Payload);
            Assert.Equal(payload, result.Payload);
            Assert.NotNull(result.Payload.SingleTouchRatio.X);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.PayloadsWithSingleTouchNullX), MemberType = typeof(TestDataGenerator))]
        public void Read_ValidNullJson_ReturnsSuccessResult(TouchSenderPayload payload)
        {
            // Log
            output.WriteLine(JsonSerializer.Serialize(payload));

            // Arrange
            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            var input = Encoding.UTF8.GetBytes(json);

            // Act
            var result = Interpreter.Read(input);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Payload);
            Assert.Equal(payload, result.Payload);
            Assert.Null(result.Payload.SingleTouchRatio.X);
        }
        [Theory]
        [InlineData("invaid json")] // no closing brace
        [InlineData("{")] // missing closing brace
        public void Read_InvalidJson_ReturnsFailureResult(string invalidJson)
        {
            // Log
            output.WriteLine(invalidJson);

            // Arrange
            var input = Encoding.UTF8.GetBytes(invalidJson);

            // Act
            var result = Interpreter.Read(input);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Payload);
            Assert.NotNull(result.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.DifferenctPayloads), MemberType = typeof(TestDataGenerator))]
        public void Equals_DifferentPayloads_ReturnsFalse(TouchSenderPayload payload1, TouchSenderPayload payload2)
        {
            // Act
            var result = payload1.Equals(payload2);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.SamePayloads), MemberType = typeof(TestDataGenerator))]
        public void Equals_SamePayloads_ReturnsTrue(TouchSenderPayload payload1, TouchSenderPayload payload2)
        {
            // Act
            var result = payload1.Equals(payload2);

            // Assert
            Assert.True(result);
        }
    }
}
