namespace ml_tds
{
    public class TrackingDataParser : MelonLoader.MelonMod
    {
        bool m_quit = false;

        MemoryMapWritter m_mapWritter = null;
        TrackingData m_trackingData;
        UnityEngine.GameObject m_trackingTarget = null;
        OpenSeeVRMDriver m_vrmDriver = null;

        public override void OnApplicationStart()
        {
            m_mapWritter = new MemoryMapWritter();
            m_mapWritter.Open("head/data");

            m_trackingData = new TrackingData();

            MelonLoader.MelonCoroutines.Start(SearchTrackingObjects());
        }
        System.Collections.IEnumerator SearchTrackingObjects()
        {
            while(m_trackingTarget == null)
            {
                m_trackingTarget = UnityEngine.GameObject.Find("VSeeFace/Tracking/IKTargetWrapper/BasicIKTarget");
                yield return null;
            }

            m_vrmDriver = m_trackingTarget.GetComponent<OpenSeeVRMDriver>();
        }

        public override void OnApplicationQuit()
        {
            if(!m_quit)
            {
                m_quit = true;

                m_mapWritter?.Close();
                m_mapWritter = null;
            }
        }

        public override void OnLateUpdate()
        {
            if((m_trackingTarget != null) && (m_vrmDriver != null))
            {
                m_trackingData.m_headPositionX = m_trackingTarget.transform.position.x;
                m_trackingData.m_headPositionY = m_trackingTarget.transform.position.y;
                m_trackingData.m_headPositionZ = m_trackingTarget.transform.position.z;
                m_trackingData.m_headRotationX = m_trackingTarget.transform.rotation.x;
                m_trackingData.m_headRotationY = m_trackingTarget.transform.rotation.y;
                m_trackingData.m_headRotationZ = m_trackingTarget.transform.rotation.z;
                m_trackingData.m_headRotationW = m_trackingTarget.transform.rotation.w;
                m_trackingData.m_gazeX = m_vrmDriver.vrcGaze.x;
                m_trackingData.m_gazeY = m_vrmDriver.vrcGaze.y;
                m_trackingData.m_blink = m_vrmDriver.vrcBlink;
                m_trackingData.m_mouthOpen = m_vrmDriver.vrcMouth.x;
                m_trackingData.m_mouthShape = m_vrmDriver.vrcMouth.y;
                m_trackingData.m_brows = m_vrmDriver.vrcBrows;

                m_mapWritter?.Write(TrackingData.ToBytes(m_trackingData));
            }
        }
    }
}
