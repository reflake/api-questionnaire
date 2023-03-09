namespace Questionnaire
{
	public class SceneContext
	{
		public static SceneContext Instance => _instance ??= new SceneContext();
		public static bool Initialized => _instance != null;

		static SceneContext _instance = null;

		public Difficulty GameDifficulty;

		SceneContext()
		{
		}
	}
}