// using System;
// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using DG.Tweening;
// using NaughtyAttributes;
// using TMPro;
// using UnityEngine;
// using Random = UnityEngine.Random;

// public class CollectingCoin : MonoBehaviour
// {
//     [Header("Money Bar")]
//     [SerializeField] private GameObject coinPrefab;
//     [SerializeField] private TextMeshProUGUI _coinText;

//     [Header("Coin value")]
//     [SerializeField] private float duration;    //duration of the animation
//     [SerializeField] private int coinAmount;    //the text value will increase for each one being duped

//     [Space]
//     [Header("Location")]
//     [SerializeField] private Transform coinParent;  //to store all the instantiated coins
//     [SerializeField] private Transform spawnLocation;
//     [SerializeField] private Transform endPosition; //the coin in the money bar

//     [Header("Spawn Offset")]
//     [SerializeField] private float spawnPosOffset;  //the small effect when spawning the coins
//     [SerializeField] private float offsetX;
//     [SerializeField] private float offsetY;
//     public int coin;   //the current coin value

//     private List<GameObject> coins = new();
//     private Tween coinReactionTween;


//     [Button()]
//     private void ResetCoin()
//     {
//         SetCoin(0);
//     }

//     [Button()]
//     public async void CollectCoins()
//     {
//         // Reset
//         for (int i = 0; i < coins.Count; i++)
//         {
//             Destroy(coins[i]);
//         }
//         coins.Clear();

//         // Spawn the coin to a specific location with random value
//         List<UniTask> spawnCoinTaskList = new();
//         for (int i = 0; i < coinAmount; i++)
//         {
//             GameObject coinInstance = Instantiate(coinPrefab, coinParent);  //instantiate the coins

//             //set the position
//             float xPosition = spawnLocation.position.x + Random.Range(-offsetX, offsetX);
//             float yPosition = spawnLocation.position.y + Random.Range(-offsetY, offsetY);
//             coinInstance.transform.position = new Vector3(xPosition, yPosition);

//             spawnCoinTaskList.Add(coinInstance.transform.DOPunchPosition(Vector3.up * spawnPosOffset, 
//                 Random.Range(0, 1f)).SetEase(Ease.InOutElastic).ToUniTask());

//             coins.Add(coinInstance);
//             await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
//         }
//         // Move all the coins to the coin label and wait till all the coins are instantiated before run the next line
//         await UniTask.WhenAll(spawnCoinTaskList);

//         // Animation the reaction when collecting coin
//         await MoveCoinsTask();
//     }


//     private async UniTask MoveCoinsTask()
//     {
//         List<UniTask> moveCoinTask = new();
//         for (int i = coins.Count - 1; i >= 0; i--)
//         {
//             moveCoinTask.Add(MoveCoinTask(coins[i]));
//             await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
//         }
//     }

//     private async UniTask MoveCoinTask(GameObject coinInstance)
//     {
//         await coinInstance.transform.DOMove(endPosition.position, duration).SetEase(Ease.InBack).ToUniTask();

//         GameObject temp = coinInstance;
//         coins.Remove(coinInstance);
//         Destroy(temp);

//         await ReactToCollectionCoin();
//         SetCoin(coin + 1);
//     }

//     private async UniTask ReactToCollectionCoin()
//     {
//         if (coinReactionTween == null)
//         {
//             coinReactionTween = endPosition.DOPunchScale(Vector3.one * 0.5f, 0.1f).SetEase(Ease.InOutElastic);
//             await coinReactionTween.ToUniTask();
//             coinReactionTween = null;
//         }
//     }
//     private void SetCoin(int value)
//     {
//         coin = value;
//         _coinText.text = coin.ToString();
//     }
// }