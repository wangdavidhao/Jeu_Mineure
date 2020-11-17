using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	public string inside;
	BoxCollider2D collider;
	[SerializeField] GameObject playerCollider;
	Vector2 position;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        changeScene();
    }

    void changeScene()
    {
    	position = playerCollider.GetComponent<Rigidbody2D>().position + new Vector2(0,0.9f);
    	if(this.collider.OverlapPoint(position))
    		SceneManager.LoadScene(inside);
    }
}
