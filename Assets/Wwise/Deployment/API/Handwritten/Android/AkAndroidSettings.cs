public class AkAndroidSettings : AkWwiseInitializationSettings.PlatformSettings
{
#if UNITY_EDITOR
	[UnityEditor.InitializeOnLoadMethod]
	private static void AutomaticPlatformRegistration()
	{
		RegisterPlatformSettingsClass<AkAndroidSettings>("Android");
	}
#endif // UNITY_EDITOR

	public AkAndroidSettings()
	{
		SetUseGlobalPropertyValue("UserSettings.m_MainOutputSettings.m_PanningRule", false);
		SetUseGlobalPropertyValue("UserSettings.m_MainOutputSettings.m_ChannelConfig.m_ChannelConfigType", false);
		SetUseGlobalPropertyValue("UserSettings.m_MainOutputSettings.m_ChannelConfig.m_ChannelMask", false);
	}

	protected override AkCommonUserSettings GetUserSettings()
	{
		return UserSettings;
	}

	protected override AkCommonAdvancedSettings GetAdvancedSettings()
	{
		return AdvancedSettings;
	}

	protected override AkCommonCommSettings GetCommsSettings()
	{
		return CommsSettings;
	}

	[System.Serializable]
	public class PlatformAdvancedSettings : AkCommonAdvancedSettings
	{
		[UnityEngine.Tooltip("Used when hardware-preferred frame size and user-preferred frame size are not compatible. If true (default), the sound engine will initialize to a multiple of the HW setting, close to the user setting. If false, the user setting is used as is, regardless of the HW preference (might incur a performance hit).")]
		public bool m_RoundFrameSizeToHardwareSize = true;

		public override void CopyTo(AkPlatformInitSettings settings)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			settings.bRoundFrameSizeToHWSize = m_RoundFrameSizeToHardwareSize;
#endif
		}
	}

	[UnityEngine.HideInInspector]
	public AkCommonUserSettings UserSettings = new AkCommonUserSettings
	{
		m_MainOutputSettings = new AkCommonOutputSettings
		{
			m_PanningRule = AkCommonOutputSettings.PanningRule.Headphones,
			m_ChannelConfig = new AkCommonOutputSettings.ChannelConfiguration
			{
				m_ChannelConfigType = AkCommonOutputSettings.ChannelConfiguration.ChannelConfigType.Standard,
				m_ChannelMask = AkCommonOutputSettings.ChannelConfiguration.ChannelMask.SETUP_STEREO,
			},
		},
	};

	[UnityEngine.HideInInspector]
	public PlatformAdvancedSettings AdvancedSettings;

	[UnityEngine.HideInInspector]
	public AkCommonCommSettings CommsSettings;
}
