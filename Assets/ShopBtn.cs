using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopBtn : MonoBehaviour
{
    public void goToShop()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
