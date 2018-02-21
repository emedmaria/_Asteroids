using UnityEngine;

public interface IViewDisplay{

	/*event EventHandler ViewHidden;
	void OnViewHidden();
	event EventHandler ViewShown;
	void OnViewShown();
	*/
	// Display implementation
	Transform ViewRoot { get; }
	void Show();
	void Hide();
}
