using System.IO.Pipes;
using System.Text;

namespace ClevoEcControlinfo
{
    /// <summary>
    /// Clevo风扇控制服务的API。
    /// </summary>
    public class ClevoEcControl
    {
        private const string PipeName = "ClevoEcPipe";

        /// <summary>
        /// ECData结构包含了风扇温度(℃)，风扇控制百分比(0~255)和预留字段。
        /// </summary>
        public struct ECData
        {
            /// <summary>
            /// 风扇温度(℃)。
            /// </summary>
            public byte Remote; // 温度
            /// <summary>
            /// 本地温度。
            /// </summary>
            public byte Local;
            /// <summary>
            /// 风扇控制百分比(占空比数据为0~255)。
            /// </summary>
            public byte FanDuty; // 风扇负载，0-255
            /// <summary>
            /// 保留字段。
            /// </summary>
            public byte Reserve;
        }

        /// <summary>
        /// 测试服务进程是否正常启动。
        /// </summary>
        public static bool IsServerStarted()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                try
                {
                    client.Connect(1000); // 1秒超时
                    byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPipeTestConnect");
                    client.Write(requestBytes, 0, requestBytes.Length);
                    return true;
                }
                catch (TimeoutException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 蓝天风扇控制程序IO初始化。
        /// </summary>
        public static bool InitIo()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPipeInitTo");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] infoBytes = new byte[1];
                int bytesRead = client.Read(infoBytes, 0, infoBytes.Length);
                return BitConverter.ToBoolean(infoBytes, 0);
            }
        }

        /// <summary>
        /// 获取EC固件版本。
        /// </summary>
        public static string GetECVersion()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPipeVersion");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] versionBytes = new byte[128];
                int bytesRead = client.Read(versionBytes, 0, versionBytes.Length);
                return Encoding.UTF8.GetString(versionBytes, 0, bytesRead);
            }
        }

        /// <summary>
        /// 获取风扇数量。
        /// </summary>
        public static int GetFanCount()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPipeFanNum");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] fanNumBytes = new byte[4];
                int bytesRead = client.Read(fanNumBytes, 0, fanNumBytes.Length);
                return BitConverter.ToInt32(fanNumBytes, 0);
            }
        }

        /// <summary>
        /// 获取CPU风扇转速，转速通过[ 2100000 / GetCpuFanRpm() ]计算。
        /// </summary>
        public static int GetCpuFanRpm()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPipeCpuFanRpm");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] rpmBytes = new byte[4];
                int bytesRead = client.Read(rpmBytes, 0, rpmBytes.Length);
                return BitConverter.ToInt32(rpmBytes, 0);
            }
        }

        /// <summary>
        /// 获取GPU风扇转速，转速通过[ 2100000 / GetGpuFanRpm() ]计算。
        /// </summary>
        public static int GetGpuFanRpm()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPipeGpuFanRpm");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] rpmBytes = new byte[4];
                int bytesRead = client.Read(rpmBytes, 0, rpmBytes.Length);
                return BitConverter.ToInt32(rpmBytes, 0);
            }
        }

        /// <summary>
        /// 获取GPU1风扇转速，转速通过[ 2100000 / GetGpu1FanRpm() ]计算。
        /// </summary>
        public static int GetGpu1FanRpm()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPipeGpu1FanRpm");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] rpmBytes = new byte[4];
                int bytesRead = client.Read(rpmBytes, 0, rpmBytes.Length);
                return BitConverter.ToInt32(rpmBytes, 0);
            }
        }

        /// <summary>
        /// 获取X72风扇转速，转速通过[ 2100000 / GetX72FanRpm() ]计算。
        /// </summary>
        public static int GetX72FanRpm()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPipeX72FanRpm");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] rpmBytes = new byte[4];
                int bytesRead = client.Read(rpmBytes, 0, rpmBytes.Length);
                return BitConverter.ToInt32(rpmBytes, 0);
            }
        }

        /// <summary>
        /// 输入风扇ID获取温度和风扇的占空比。
        /// </summary>
        public static ECData GetTempFanDuty(int fan_id)
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPipeTempFanDuty");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] fanIdBytes = BitConverter.GetBytes(fan_id);
                client.Write(fanIdBytes, 0, fanIdBytes.Length);

                ECData data = new ECData();
                byte[] dataBytes = new byte[1];
                int bytesRead;

                bytesRead = client.Read(dataBytes, 0, dataBytes.Length);
                data.Remote = dataBytes[0];

                bytesRead = client.Read(dataBytes, 0, dataBytes.Length);
                data.Local = dataBytes[0];

                bytesRead = client.Read(dataBytes, 0, dataBytes.Length);
                data.FanDuty = dataBytes[0];

                bytesRead = client.Read(dataBytes, 0, dataBytes.Length);
                data.Reserve = dataBytes[0];

                return data;
            }
        }

        /// <summary>
        /// 设置风扇控制占空比，输入风扇ID和控制的数值(占空比数据为0~255)。
        /// </summary>
        public static void SetFanDuty(int fan_id, int duty)
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPiceSetFanDuty");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] fanIdBytes = BitConverter.GetBytes(fan_id);
                client.Write(fanIdBytes, 0, fanIdBytes.Length);

                byte[] dutyBytes = BitConverter.GetBytes(duty);
                client.Write(dutyBytes, 0, dutyBytes.Length);
            }
        }

        /// <summary>
        /// 设置风扇控制为自动(通过EC自行管理)。
        /// </summary>
        public static void SetFanDutyAuto(int fan_id)
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("ClevoEcPiceSetFanAuto");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] fanIdBytes = BitConverter.GetBytes(fan_id);
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
        }

        /// <summary>
        /// 查询看门狗程序是否运行。
        /// </summary>
        public static bool IsWatchDogStarted()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("WatchDogInitTo");
                client.Write(requestBytes, 0, requestBytes.Length);

                byte[] buffer = new byte[1];
                client.Read(buffer, 0, buffer.Length);
                return BitConverter.ToBoolean(buffer, 0);
            }
        }

        /// <summary>
        /// 启动看门狗程序。
        /// </summary>
        public static void SetWatchDogStarted()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("WatchDogStart");
                client.Write(requestBytes, 0, requestBytes.Length);
            }
        }

        /// <summary>
        /// 关闭看门狗程序。
        /// </summary>
        public static void SetWatchDogClosed()
        {
            using (var client = new NamedPipeClientStream(PipeName))
            {
                client.Connect(1000);
                byte[] requestBytes = Encoding.UTF8.GetBytes("WatchDogClose");
                client.Write(requestBytes, 0, requestBytes.Length);
            }
        }
    }
}