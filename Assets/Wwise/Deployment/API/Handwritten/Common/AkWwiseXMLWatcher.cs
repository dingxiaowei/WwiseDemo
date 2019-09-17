#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

public class AkWwiseXMLWatcher
{
	private static readonly AkWwiseXMLWatcher instance = new AkWwiseXMLWatcher();
	public static AkWwiseXMLWatcher Instance { get { return instance; } }

	private readonly string SoundBankFolder;
	private readonly System.IO.FileSystemWatcher XmlWatcher;

	private bool fireEvent = false;

	public event System.Action XMLUpdated;

	public System.Func<bool> PopulateXML;

	private AkWwiseXMLWatcher()
	{
		XmlWatcher = new System.IO.FileSystemWatcher();
		SoundBankFolder = AkBasePathGetter.GetSoundbankBasePath();

		try
		{
			XmlWatcher.Path = SoundBankFolder;
			XmlWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;

			// Event handlers that are watching for specific event
			XmlWatcher.Created += RaisePopulateFlag;
			XmlWatcher.Changed += RaisePopulateFlag;

			XmlWatcher.Filter = "*.xml";
			XmlWatcher.IncludeSubdirectories = true;
			XmlWatcher.EnableRaisingEvents = true;
		}
		catch (System.Exception)
		{
			// Deliberately left empty
		}

		UnityEditor.EditorApplication.update += onEditorUpdate;
	}

	void onEditorUpdate()
	{
		if (!fireEvent)
			return;

		fireEvent = false;

		var populate = PopulateXML;
		if (populate == null || !populate())
			return;

		var callback = XMLUpdated;
		if (callback != null)
		{
			callback();
		}

		AkBankManager.ReloadAllBanks();
	}

	private void RaisePopulateFlag(object sender, System.IO.FileSystemEventArgs e)
	{
		// Signal the main thread it's time to populate (cannot run populate somewhere else than on main thread)
		fireEvent = true;
	}
}
#endif