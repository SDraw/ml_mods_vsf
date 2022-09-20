using System;
using System.Runtime.InteropServices;

struct TrackingData
{
    public float m_headPositionX;
    public float m_headPositionY;
    public float m_headPositionZ;
    public float m_headRotationX;
    public float m_headRotationY;
    public float m_headRotationZ;
    public float m_headRotationW;
    public float m_gazeX;
    public float m_gazeY;
    public float m_blink;
    public float m_mouthOpen;
    public float m_mouthShape;
    public float m_brows;

    static public byte[] ToBytes(TrackingData p_faceData)
    {
        int l_size = Marshal.SizeOf(p_faceData);
        byte[] l_arr = new byte[l_size];

        IntPtr ptr = Marshal.AllocHGlobal(l_size);
        Marshal.StructureToPtr(p_faceData, ptr, true);
        Marshal.Copy(ptr, l_arr, 0, l_size);
        Marshal.FreeHGlobal(ptr);
        return l_arr;
    }

    static public TrackingData ToObject(byte[] p_buffer)
    {
        TrackingData l_trackingData = new TrackingData();

        int l_size = Marshal.SizeOf(l_trackingData);
        IntPtr l_ptr = Marshal.AllocHGlobal(l_size);

        Marshal.Copy(p_buffer, 0, l_ptr, l_size);

        l_trackingData = (TrackingData)Marshal.PtrToStructure(l_ptr, l_trackingData.GetType());
        Marshal.FreeHGlobal(l_ptr);

        return l_trackingData;
    }
}
