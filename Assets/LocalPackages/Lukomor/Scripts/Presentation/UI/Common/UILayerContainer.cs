using UnityEngine;

namespace WKosArch.Presentation.Common {
	public class UILayerContainer : MonoBehaviour {
		[SerializeField] private UILayer _layer;

		public UILayer layer => _layer;
	}
}