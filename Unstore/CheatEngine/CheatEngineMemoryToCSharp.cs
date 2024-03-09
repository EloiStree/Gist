using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;


[System.Serializable]
public class WowAddressOfPositionString
{
    public string m_localAliasName="";
    public string m_processId = "0x1F0FFF";
    public string m_xHorizontalMiniMapAddress = "0x22B2085F8C8";
    public string m_yVerticalMiniMapAddress = "0x22B2085F8C8";
    public string m_zHeightAddress = "0x22B2085F8C8";
}



public class MemoryFloatFetcher {


    const int PROCESS_ALL_ACCESS = 0x1F0FFF;

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr hObject);


    public ProcessOpenHandler m_process ;
    public IntPtr m_mapAddress;//= 0x22B2085F8C8;

    byte[] m_bufferByte = new byte[4];
    float m_value = 0;

    public MemoryFloatFetcher(ProcessOpenHandler processHandle, IntPtr mapAddress0x) {
        m_process = processHandle; 
        m_mapAddress = mapAddress0x;
    }

    public void Get(out bool isFound, out float value) {

        isFound = false;
        value = 0;

        if (ReadProcessMemory(m_process, m_mapAddress, m_bufferByte, (uint)m_bufferByte.Length, out int intByteValue))
        {
            value = BitConverter.ToSingle(intByteValue, 0);
            isFound = true;
        }

        m_value = value;
    }
}



[System.Serializable]
public class MemoryVector3Fetcher
{
    public MemoryFloatFetcher m_xHorizontalMiniMapAddress;
    public MemoryFloatFetcher m_yVerticalMiniMapAddress;
    public MemoryFloatFetcher m_zHeightAddress;
}

[System.Serializable]
public class NamedMemoryFloatFetcherVector3
{
    public string m_localAliasName = "";
    public MemoryVector3Fetcher m_vector3MapAddress;
}

[System.Serializable]
public class NamedMemoryFloatFetcherFloat

{
    public string m_localAliasName = "";
    public MemoryFloatFetcher m_floatMapAddress;
}


[System.Serializable]
public class GateUDPOut
{
    public string m_address = "127.0.0.1";
    public int m_port = 4501;
}
[System.Serializable]
public class GateUDPIn
{
    public int m_port = 4501;
}





public class ProcessOpenHandler {

    public IntPtr m_processHandle= IntPtr.Zero;
    public ProcessOpenHandler(int processId0x) {
         m_processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, processId0x);
    }
    public bool IsProcessOpen() { return m_processHandle == IntPtr.Zero; }
    public IntPtr GetHandlerPointer() { return m_processHandle; }

}

class Program
{
   

    static void Main()
    {

        // Goblin start zone hearthstone:
        // x horizontal 1361,3
        // y Vertical   -8423,876 
        // z height     -8423,876
        //  

        ProcessOpenHandler handler = new ProcessOpenHandler(0x6EF4);
        MemoryFloatFetcher floatXP = new MemoryFloatFetcher(handler, (IntPtr)0x2135EF688BC);
        MemoryFloatFetcher floatX = new MemoryFloatFetcher(handler, (IntPtr)0x2131FF3F374);
        MemoryFloatFetcher floatZ = new MemoryFloatFetcher(handler, (IntPtr)0x2131FDC2D30);
        MemoryFloatFetcher floatY = new MemoryFloatFetcher(handler, (IntPtr)0x2131FDC2D38);


        while (true) {
            bool found;
            floatXP.Get(out  found, out float xp);
            floatX.Get(out  found, out float x);
            floatZ.Get(out  found, out float z);
            floatY.Get(out  found, out float y);
            Console.WriteLine(string.Format("XP{0}  H{1} V{2} Top{3}", xp, x, z, y));
            // Wait for 0.1 seconds
            Thread.Sleep(100);
        }
    }
}



public class FirstAttemptDemo {
    const int PROCESS_ALL_ACCESS = 0x1F0FFF;

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr hObject);

    public void FirstTestLoop()
    {

        // Text Location 0x213EBD765F0 text
        // Process 0x00000849
        // Memory addresses to read
        IntPtr addressToReadHorizontal = (IntPtr)0x22B2085F8C8; // Replace with your actual memory address for horizontal
        IntPtr addressToReadVertical = (IntPtr)0x22B203E4370; // Replace with your actual memory address for vertical
        IntPtr addressToReadHeight = (IntPtr)0x22B203E4370; // Replace with your actual memory address for height

        // Buffers to store read data
        byte[] bufferHorizontal = new byte[4]; // Assuming a 4-byte float for horizontal
        byte[] bufferVertical = new byte[4]; // Assuming a 4-byte float for vertical
        byte[] bufferHeight = new byte[4]; // Assuming a 4-byte float for height

        float valueHorizontal = 0;
        float valueVertical = 0;
        float valueHeight = 0;



        try
        {
            while (true)
            {

                // Read memory at the horizontal address
                int bytesReadHorizontal;


                if (ReadProcessMemory(processHandle, addressToReadHorizontal, bufferHorizontal, (uint)bufferHorizontal.Length, out bytesReadHorizontal))
                {
                    float horizontalValue = BitConverter.ToSingle(bufferHorizontal, 0);
                    valueHorizontal = horizontalValue;
                }
                else
                {

                }



                // Read memory at the vertical address
                int bytesReadVertical;
                if (ReadProcessMemory(processHandle, addressToReadVertical, bufferVertical, (uint)bufferVertical.Length, out bytesReadVertical))
                {
                    float verticalValue = BitConverter.ToSingle(bufferVertical, 0);

                    valueVertical = verticalValue;
                }
                else
                {
                }




                // Read memory at the height address
                int bytesReadHeight;
                if (ReadProcessMemory(processHandle, addressToReadHeight, bufferHeight, (uint)bufferHeight.Length, out bytesReadHeight))
                {
                    float heightValue = BitConverter.ToSingle(bufferHeight, 0);

                    valueHeight = heightValue;
                }
                else
                {
                }
                Console.WriteLine($"Horizontal Value: {valueHorizontal} : {valueVertical} : {valueHeight} ");


                // Wait for 0.1 seconds
                Thread.Sleep(100);

            }
        }
        finally
        {
            // Close the process handle when done
            CloseHandle(processHandle);
        }
    }
}