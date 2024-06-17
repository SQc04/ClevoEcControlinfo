using System.IO.Pipes;
using System.Text;

namespace ClevoEcControlinfo
{
    public class ClevoEcControl
    {
        public struct ECData
        {
            public byte Remote; // 温度
            public byte Local;
            public byte FanDuty; // 风扇负载，0-255
            public byte Reserve;
        }
        public bool IsServerStarted()
        {
            using (var client = new NamedPipeClientStream("ClevoEcPipe"))
            {
                try
                {
                    client.Connect(0);  // 尝试立即连接，不等待
                    return true;  // 如果连接成功，返回 true
                }
                catch (TimeoutException)
                {
                    return false;  // 如果连接超时（服务器未启动），返回 false
                }
            }
        }

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
    }
}
