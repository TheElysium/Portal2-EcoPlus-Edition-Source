using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    // Script qui sert à passer au niveau suivant et à lancer la transition quand le joueur rentre dans un trigger

    public Animator transition;

    public float transitionTime = 1f; // la durée de la transition

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // si le joueur entre dans le trigger
        {
            LoadNextLevel(); // On lance le prochain niveau
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)); // on appelle la fonction avec l'indice du prochain niveau
    }

    IEnumerator LoadLevel(int levelIndex) // fonction qui charge un niveau avec son indice 
    {
        transition.SetTrigger("Start"); // on lance la transition

        yield return new WaitForSeconds(transitionTime); // on attend la fin de la transition

        SceneManager.LoadSceneAsync(levelIndex); // on charge le niveau avec l'indice passé en paramètre
    }
}