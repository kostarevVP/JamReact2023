using Cysharp.Threading.Tasks;

namespace WKosArch.Domain.Contexts
{
	public interface IContext
	{
		bool IsReady { get; }

        UniTask InitializeAsync();
		void Destroy();
	}
}