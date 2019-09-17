public class AkiOSSettings : AkWwiseInitializationSettings.PlatformSettings
{
#if UNITY_EDITOR
	[UnityEditor.InitializeOnLoadMethod]
	private static void AutomaticPlatformRegistration()
	{
		RegisterPlatformSettingsClass<AkiOSSettings>("iOS");
	}
#endif // UNITY_EDITOR

	public AkiOSSettings()
	{
		// Valid for iOS not necessarily for tvOS
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
		public enum Category
		{
			Ambient,
			SoloAmbient,
			PlayAndRecord,
		}

		[UnityEngine.Tooltip("The IDs of the iOS audio session categories, useful for defining app-level audio behaviours such as inter-app audio mixing policies and audio routing behaviours.These IDs are functionally equivalent to the corresponding constants defined by the iOS audio session service back-end (AVAudioSession). Refer to Xcode documentation for details on the audio session categories.")]
		public Category m_AudioSessionCategory = Category.SoloAmbient;

		public enum CategoryOptions
		{
			MixWithOthers = 1,
			DuckOthers = 2,
			AllowBluetooth = 4,
			DefaultToSpeaker = 8,
		}

		[UnityEngine.Tooltip("The IDs of the iOS audio session category options, used for customizing the audio session category features. These IDs are functionally equivalent to the corresponding constants defined by the iOS audio session service back-end (AVAudioSession). Refer to Xcode documentation for details on the audio session category options.")]
		[AkEnumFlag(typeof(CategoryOptions))]
		public CategoryOptions m_AudioSessionCategoryOptions = CategoryOptions.DuckOthers;

		public enum Mode
		{
			Default,
			VoiceChat,
			GameChat,
			VideoRecording,
			Measurement,
			MoviePlayback,
			VideoChat,
		}

		[UnityEngine.Tooltip("The IDs of the iOS audio session modes, used for customizing the audio session for typical app types. These IDs are functionally equivalent to the corresponding constants defined by the iOS audio session service back-end (AVAudioSession). Refer to Xcode documentation for details on the audio session category options.")]
		public Mode m_AudioSessionMode = Mode.Default;

		public override void CopyTo(AkPlatformInitSettings settings)
		{
#if (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
			settings.audioSession.eCategory = (AkAudioSessionCategory)m_AudioSessionCategory;
			settings.audioSession.eCategoryOptions = (AkAudioSessionCategoryOptions)m_AudioSessionCategoryOptions;
			settings.audioSession.eMode = (AkAudioSessionMode)m_AudioSessionMode;
#endif
		}
	}

	[UnityEngine.HideInInspector]
	public AkCommonUserSettings UserSettings = new AkCommonUserSettings
	{
		m_MainOutputSettings = new AkCommonOutputSettings
		{
			// TODO: This might not be true... need to find out from core.
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
