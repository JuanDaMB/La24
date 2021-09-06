using IndieLevelStudio.BetsModule.Controllers;
using IndieLevelStudio.BoardModule.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.BoardModule.Controllers
{
    public class BoardController : MonoBehaviour
    {
        public event Action OnCollecttedMoney;

        public event Action<BetInfo> OnLastBetRemoved;

        public event Action<BetInfo> OnBetPlaced;

        public event Action<int> OnIndividualBetRemoved;

        public event Action<int> OnIndividualBetPlaced;

        public event Action<Dictionary<Bet, int>> OnBetsFinished;

        public event Action<Bet> OnClickedBet;

        public event Action<bool> OnBetsOpened;
        
        public event Action AllBetsRemoved;

        public List<Bet> Bets
        {
            get;
            private set;
        }

        private List<Bet> organizedBets;

        private Dictionary<Bet, int> userBets;

        private Dictionary<Bet, int> lastBets;

        [SerializeField]
        private Token tokenPrefab;

        [SerializeField]
        private Transform betsPoint;

        [SerializeField]
        private Transform betsContainer;

        [SerializeField]
        private Button buttonRemoveLast;

        [SerializeField]
        private Button buttonRemoveAll;

        [SerializeField]
        public Button buttonReplayLastBet;

        private BetInfo betInfo;

        public bool IsBetsOpened { get; private set; }

        public void Setup(bool isPromotional)
        {
            Bets = new List<Bet>(betsContainer.GetComponentsInChildren<Bet>());
            userBets = new Dictionary<Bet, int>();
            organizedBets = new List<Bet>();

            Bets.ForEach(bet =>
            {
                bet.Setup();
                SetRewardFeatures(bet);
                bet.button.onClick.AddListener(() =>
                {
                    if (OnClickedBet != null)
                        OnClickedBet(bet);
                });
            });
            ConfigurePromotional(isPromotional);
        }

        private void SetRewardFeatures(Bet bet)
        {
            switch (bet.RewardType)
            {
                case 1:
                    bet.SetRewardFeatures("Octeto", GlobalObjects.PrizesData.maxBetTypes.Octeto);
                    break;

                case 2:
                    bet.SetRewardFeatures("Cuadrado", GlobalObjects.PrizesData.maxBetTypes.Cuadrado);
                    break;

                case 3:
                    bet.SetRewardFeatures("Pleno", GlobalObjects.PrizesData.maxBetTypes.Pleno);
                    break;

                case 4:
                    bet.SetRewardFeatures("Medio", GlobalObjects.PrizesData.maxBetTypes.Medio);
                    break;

                case 5:
                    bet.SetRewardFeatures("Altos_Bajos", GlobalObjects.PrizesData.maxBetTypes.Altos_Bajos);
                    break;

                case 6:
                    bet.SetRewardFeatures("Par_Impar", GlobalObjects.PrizesData.maxBetTypes.Par_Impar);
                    break;

                case 7:
                    bet.SetRewardFeatures("Rojo_Negro", GlobalObjects.PrizesData.maxBetTypes.Rojo_Negro);
                    break;

                case 8:
                    bet.SetRewardFeatures("Fila", GlobalObjects.PrizesData.maxBetTypes.Fila);
                    break;
            }
        }

        private void ConfigurePromotional(bool isPromotional)
        {
            if (isPromotional)
            {
                buttonRemoveAll.gameObject.SetActive(false);
                buttonRemoveLast.gameObject.SetActive(false);
                buttonReplayLastBet.gameObject.SetActive(false);
            }
            else
            {
                foreach (Bet b in userBets.Keys)
                    b.Token.UpdateValue();

                buttonRemoveAll.onClick.AddListener(RemoveAllBetsButton);
                buttonRemoveLast.onClick.AddListener(RemoveLastBet);
                buttonReplayLastBet.onClick.AddListener(ReplayLastBet);

                OnClickedBet += OnClickBet;
            }
        }

        private void RemoveAllBetsButton()
        {
            RemoveAllBets();
        }

        public void OpenBets(bool isOpen)
        {
            IsBetsOpened = isOpen;
            if (OnBetsOpened != null)
                OnBetsOpened(IsBetsOpened);
        }

        public void OnClickBet(Bet bet)
        {
            if (ModulesGlobalObjects.UserMoney < betInfo.amount * ModulesGlobalObjects.CurrentDenomination)
                return;

            int dummyBet = ModulesGlobalObjects.UserBet + betInfo.amount * ModulesGlobalObjects.CurrentDenomination;
            if (dummyBet > GlobalObjects.MaxBet)
                return;

            
            int betPerZone = BetPerZone(bet,betInfo.amount);

            if (betPerZone * ModulesGlobalObjects.CurrentDenomination > bet.MaxBet) {
                print ("MaxBet Reached: " + bet.MaxBet);
                return;
            }
            
            if (OnBetPlaced != null)
                OnBetPlaced(betInfo);

            if (userBets.ContainsKey(bet))
            {
                userBets[bet] += betInfo.amount;				
            }
            else
            {
                userBets.Add(bet, betInfo.amount);
            }

            if (bet.Token == null)
            {
                Token t = Instantiate(tokenPrefab, bet.transform, false);
                bet.SetMoneyToken(t);
            }

            bet.betsInBet.Add(betInfo);
            organizedBets.Add(bet);

            PlaySound.audios.PlayFX("Sonido Fichas");
            
            bet.Token.SetText(userBets[bet]);
        }

        private int BetPerZone(Bet bet, int betinfo)
        {
            int betPerZone = 0;
            if (userBets.ContainsKey(bet))
            {
                betPerZone = userBets[bet] + betinfo;
            }
            else
            {
                betPerZone = betinfo;
            }
            return betPerZone;
        }

        public void ClearBoardAnimated(out float time)
        {
            time = 0;
            if (userBets.Count == 0)
                return;

            float delay = 0;
            foreach (KeyValuePair<Bet, int> element in userBets)
            {
                int rand = UnityEngine.Random.Range(1, 3);
                PlaySound.audios.PlayFX("Woosh Kill " + rand, 0.4f, delay);

                LeanTween.move(element.Key.Token.gameObject, betsPoint.position, 0.15f).setDelay(delay).setOnComplete(() =>
                {
                    element.Key.RemoveBet();
                    userBets.Remove(element.Key);
                });

                delay += 0.15f;
                time += 0.15f;
            }

            userBets.Clear();
            organizedBets.Clear();
            
            Bets.ForEach (o => o.betsInBet.Clear ());
        }

        public void RemoveAllBets(bool clearBetsInBet = true)
        {
            organizedBets.ForEach(o => o.betsInBet.Clear());
            organizedBets.Clear();

            Bets.ForEach (o => o.betsInBet.Clear ());
            
            foreach (KeyValuePair<Bet, int> element in userBets)
            {
                element.Key.RemoveBet();

                if (OnIndividualBetRemoved != null)
                    OnIndividualBetRemoved(element.Value);
            }
            userBets.Clear();

            if (AllBetsRemoved != null)
            {
                AllBetsRemoved();
            }
        }

        private void RemoveLastBet()
        {
            if (organizedBets.Count <= 0)
            {
                if (AllBetsRemoved != null)
                {
                    AllBetsRemoved();
                }
                userBets.Clear ();
                return;
            }

            Bet lastBet = organizedBets[organizedBets.Count - 1];

            if (lastBet.betsInBet.Count > 0)
            {
                BetInfo lastBetInfo = lastBet.betsInBet[lastBet.betsInBet.Count - 1];
                userBets[lastBet] -= lastBetInfo.amount;

                lastBet.betsInBet.Remove(lastBetInfo);
                lastBet.Token.SetText(userBets[lastBet]);

                if (lastBet.betsInBet.Count == 0)
                {
                    lastBet.RemoveBet();
                    userBets.Remove(lastBet);

                    organizedBets.RemoveAt(organizedBets.Count - 1);
                    if (userBets.Count <= 0)
                    {
                        if (AllBetsRemoved != null)
                        {
                            AllBetsRemoved();
                        }
                        userBets.Clear ();
                    }
                }
                if (OnLastBetRemoved != null)
                    OnLastBetRemoved(lastBetInfo);
            }
            else
            {
                lastBet.RemoveBet();
                userBets.Remove(lastBet);

                organizedBets.RemoveAt(organizedBets.Count - 1);

                if (lastBet.Token != null && OnLastBetRemoved != null)
                    OnLastBetRemoved(new BetInfo(lastBet.Token.Value));
            }
            if (userBets.Count <= 0)
            {
                if (AllBetsRemoved != null)
                {
                    AllBetsRemoved();
                }
                userBets.Clear ();
            }
        }

        private void CollectMoney()
        {
            if (IsBetsOpened)
                return;

            organizedBets.ForEach(o => o.betsInBet.Clear());
            organizedBets.Clear();

            foreach (KeyValuePair<Bet, int> element in userBets)
                Destroy(element.Key.Token.gameObject);

            userBets.Clear();

            if (OnCollecttedMoney != null)
                OnCollecttedMoney();
        }

        private void ReplayLastBet()
        {
            if (ModulesGlobalObjects.LastBetRegister.Count == 0)
                return;

            int lastBetTotalValue = BetListTotalValues(ModulesGlobalObjects.LastBetRegister);
            float maxBet;
            
            maxBet = GlobalObjects.MaxBet;
          
            if (lastBetTotalValue > ModulesGlobalObjects.UserMoney)
                return;
            foreach (int value in userBets.Values)
            {
                lastBetTotalValue += value * ModulesGlobalObjects.CurrentDenomination;
            }
            
            if (lastBetTotalValue > maxBet) {
                print ("MaxBet REACHED: " + maxBet);
                return;
            }

            foreach (KeyValuePair<Bet, int> element in ModulesGlobalObjects.LastBetRegister)
            {
                int betPerZone = BetPerZone(element.Key, element.Value);
                
                if (betPerZone * ModulesGlobalObjects.CurrentDenomination > element.Key.MaxBet) {
                    print ("MaxBet Reached: " + element.Key.MaxBet);
                }
                else
                {
                    if (element.Key.Token == null)
                    {
                        Token t = Instantiate(tokenPrefab, element.Key.transform, false);
                        element.Key.SetMoneyToken(t);
                    }
                    if (userBets.ContainsKey(element.Key))
                        userBets[element.Key] += element.Value;
                    else
                        userBets.Add(element.Key, element.Value);

                    element.Key.betsInBet.Add(new BetInfo(element.Value));
                    organizedBets.Add(element.Key);

                    if (OnIndividualBetPlaced != null)
                        OnIndividualBetPlaced(element.Value);

                    element.Key.Token.SetText(userBets[element.Key]);
                }
            }
        }

        private int BetListTotalValues(Dictionary<Bet, int> comparer)
        {
            int betsTotalValues = 0;
            lastBets = new Dictionary<Bet, int>();
            Bets.ForEach(b =>
            {
                if (comparer.ContainsKey(b))
                {
                    lastBets[b] = comparer[b];
                    betsTotalValues += lastBets[b] * ModulesGlobalObjects.CurrentDenomination;
                }
            });
            return betsTotalValues;
        }

        public void BetsFinished()
        {
            IsBetsOpened = false;
            
            if (userBets.Count == 0)
            {
                Debug.LogWarning("User does not make a bet");
                return;
            }

            int betsTotalValue = BetListTotalValues(userBets);
            if (betsTotalValue < GlobalObjects.MinBet)
            {
                Alert.Instance.Show("Alerta", "El monto no cumple con la apuesta minima requerida: " + GlobalObjects.MinBet, 4);
                return;
            }
            ModulesGlobalObjects.LastBetRegister = new Dictionary<Bet, int>(userBets);

            OnBetsFinished?.Invoke(userBets);
        }

#if UNITY_EDITOR

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                ModulesGlobalObjects.LastBetRegister = new Dictionary<Bet, int>(userBets);
                Debug.Log("Last bet register updated");
            }
        }

#endif

        public void UpdateTokensValue()
        {
            foreach (Bet b in userBets.Keys)
            {
                if (b.Token != null)
                    b.Token.UpdateValue();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (Transform s in betsContainer)
                Gizmos.DrawSphere(s.position, 0.1f);
        }

        public void BetChanged(BetInfo betInfo)
        {
            this.betInfo = betInfo;
        }
    }
}