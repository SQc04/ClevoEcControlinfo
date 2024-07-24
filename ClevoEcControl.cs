using System.IO.Pipes;
using System.Text;

namespace ClevoEcControlinfo
{
    /// <summary>
    /// Clevo风扇控制服务的API。
    /// </summary>
    public class ClevoEcControl
    {
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
            /// 
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
        /// IsServerStarted测试服务进程是否正常启动
        /// </summary>
        public static bool IsServerStarted()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                try
                {
                    client.Connect(0);  // 尝试立即连接，不等待
                    byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPipeTestConnect");
                    client.Write(fanIdBytes, 0, fanIdBytes.Length);
                    return true;  // 如果连接成功，返回 true
                }
                catch (TimeoutException)
                {
                    return false;  // 如果连接超时（服务器未启动），返回 false
                }
            }
        }

        /// <summary>
        /// 蓝天风扇控制程序IO初始化
        /// </summary>
        public static bool InitIo()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPipeInitTo");
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
            using (var client = new NamedPipeClientStream("ClevoEcPipeInitTo"))
            {
                client.Connect();

                byte[] infoBytes = new byte[1];
                int bytesRead = client.Read(infoBytes, 0, infoBytes.Length);
                bool isInitialized = BitConverter.ToBoolean(infoBytes, 0);

                return isInitialized;
            }
        }

        /// <summary>
        /// 获取EC固件版本
        /// </summary>
        public static string GetECVersion()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPipeVersion");
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
            using (var client = new NamedPipeClientStream("ClevoEcPipeVersion"))
            {
                client.Connect();

                byte[] versionBytes = new byte[128];
                int bytesRead = client.Read(versionBytes, 0, versionBytes.Length);
                string version = Encoding.UTF8.GetString(versionBytes, 0, bytesRead);

                return version;
            }
        }

        /// <summary>
        /// 获取风扇数量
        /// </summary>
        public static int GetFanCount()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPipeFanNum");
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
            using (var client = new NamedPipeClientStream("ClevoEcPipeFanNum"))
            {
                client.Connect();

                byte[] fanNumBytes = new byte[4];
                int bytesRead = client.Read(fanNumBytes, 0, fanNumBytes.Length);
                int fanNum = BitConverter.ToInt32(fanNumBytes, 0);

                return fanNum;
            }
        }

        /// <summary>
        /// 获取Cpu风扇转速，转速通过[ 2100000 / GetCpuFanRpm() ]计算
        /// </summary>
        public static int GetCpuFanRpm()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPipeCpuFanRpm");
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
            using (var client = new NamedPipeClientStream("ClevoEcPipeCpuFanRpm"))
            {
                client.Connect();

                byte[] cpuFanRpmBytes = new byte[4];
                int bytesRead = client.Read(cpuFanRpmBytes, 0, cpuFanRpmBytes.Length);
                int cpuFanRpm = BitConverter.ToInt32(cpuFanRpmBytes, 0);

                return cpuFanRpm;
            }
        }

        /// <summary>
        /// 获取Gpu风扇转速，转速通过[ 2100000 / GetGpuFanRpm() ]计算
        /// </summary>
        public static int GetGpuFanRpm()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPipeGpuFanRpm");
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
            using (var client = new NamedPipeClientStream("ClevoEcPipeGpuFanRpm"))
            {
                client.Connect();

                byte[] gpuFanRpmBytes = new byte[4];
                int bytesRead = client.Read(gpuFanRpmBytes, 0, gpuFanRpmBytes.Length);
                int gpuFanRpm = BitConverter.ToInt32(gpuFanRpmBytes, 0);

                return gpuFanRpm;
            }
        }

        /// <summary>
        /// 获取Gpu1风扇转速，转速通过[ 2100000 / GetGpu1FanRpm() ]计算
        /// </summary>
        public static int GetGpu1FanRpm()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPipeGpu1FanRpm");
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
            using (var client = new NamedPipeClientStream("ClevoEcPipeGpu1FanRpm"))
            {
                client.Connect();

                byte[] gpu1FanRpmBytes = new byte[4];
                int bytesRead = client.Read(gpu1FanRpmBytes, 0, gpu1FanRpmBytes.Length);
                int gpu1FanRpm = BitConverter.ToInt32(gpu1FanRpmBytes, 0);

                return gpu1FanRpm;
            }
        }

        /// <summary>
        /// 获取X72风扇转速，转速通过[ 2100000 / GetX72FanRpm() ]计算
        /// </summary>
        public static int GetX72FanRpm()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPipeX72FanRpm");
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
            using (var client = new NamedPipeClientStream("ClevoEcPipeX72FanRpm"))
            {
                client.Connect();

                byte[] x72FanRpmBytes = new byte[4];
                int bytesRead = client.Read(x72FanRpmBytes, 0, x72FanRpmBytes.Length);
                int x72FanRpm = BitConverter.ToInt32(x72FanRpmBytes, 0);

                return x72FanRpm;
            }
        }

        /// <summary>
        /// 输入风扇ID获取温度和风扇的占空比
        /// </summary>
        public static ECData GetTempFanDuty(int fan_id)
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPipeTempFanDuty");
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
            using (var client = new NamedPipeClientStream("ClevoEcPipeTempFanDuty"))
            {
                client.Connect();

                // 发送 fan_id
                byte[] fanIdBytes = BitConverter.GetBytes(fan_id);
                client.Write(fanIdBytes, 0, fanIdBytes.Length);

                // 接收 ECData
                ECData data = new ECData();
                byte[] dataBytes = new byte[4];
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
        /// 设置风扇控制占空比，输入风扇id和控制的数值(占空比数据为0~255)
        /// </summary>
        public static void SetFanDuty(int fan_id, int duty)
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPiceSetFanDuty");
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
            using (var client = new NamedPipeClientStream("ClevoEcPiceSetFanDuty"))
            {
                client.Connect();

                // 发送 fan_id
                byte[] fanIdBytes = BitConverter.GetBytes(fan_id);
                client.Write(fanIdBytes, 0, fanIdBytes.Length);

                // 发送 duty
                byte[] dutyBytes = BitConverter.GetBytes(duty);
                client.Write(dutyBytes, 0, fanIdBytes.Length);
            }
        }

        /// <summary>
        /// 设置风扇控制为自动(通过EC自行管理)
        /// </summary>
        public static void SetFanDutyAuto(int fan_id)
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] fanIdBytes = System.Text.Encoding.Default.GetBytes("ClevoEcPiceSetFanAuto");
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
            using (var client = new NamedPipeClientStream("ClevoEcPiceSetFanAuto"))
            {
                client.Connect();

                // 发送 fan_id
                byte[] fanIdBytes = BitConverter.GetBytes(fan_id);
                client.Write(fanIdBytes, 0, fanIdBytes.Length);
            }
        }

        /// <summary>
        /// 查询看门狗程序是否运行
        /// </summary>
        public static bool IsWatchDogStarted()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] idBytes = System.Text.Encoding.Default.GetBytes("WatchDogInitTo");
                client.Write(idBytes, 0, idBytes.Length);

                
            }
            using (var client = new NamedPipeClientStream("WatchDogInitTo"))
            {
                client.Connect();
                // 创建一个缓冲区来存储从服务器接收到的数据
                byte[] buffer = new byte[4]; // 布尔值在 .NET 中占用 4 字节
                client.Read(buffer, 0, buffer.Length);

                // 将接收到的字节转换回布尔值
                bool watchDoginfo = BitConverter.ToBoolean(buffer, 0);
                return watchDoginfo;
            }
        }

        /// <summary>
        /// 启动看门狗程序
        /// </summary>
        public static void SetWatchDogStarted()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] idBytes = System.Text.Encoding.Default.GetBytes("WatchDogStart");
                client.Write(idBytes, 0, idBytes.Length);
            }
        }

        /// <summary>
        /// 关闭看门狗程序
        /// </summary>
        public static void SetWatchDogClosed()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                client.Connect();
                byte[] idBytes = System.Text.Encoding.Default.GetBytes("WatchDogClose");
                client.Write(idBytes, 0, idBytes.Length);
            }
        }

    }
}
