public sealed class ProjectContext
{
    public ProjectContext(IAudioService audioService, ISaveLoadService saveLoadService)
    {
        AudioService = audioService;
        SaveLoadService = saveLoadService;
    }

    public IAudioService AudioService { get; }
    public ISaveLoadService SaveLoadService { get; }
}
