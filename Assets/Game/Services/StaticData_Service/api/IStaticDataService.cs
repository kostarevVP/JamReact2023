using WKosArch.Domain.Features;
using System.Collections.Generic;
using WKosArch.Services.UIService;

namespace WKosArch.Services.StaticDataServices
{
	public interface IStaticDataService : IFeature
	{
		GameProgressConfig GameProgressConfig { get; }
		Dictionary<string, UISceneConfig> SceneConfigsMap { get; }
	} 
}