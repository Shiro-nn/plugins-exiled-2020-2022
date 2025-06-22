using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MEC
{
	public class Timing : MonoBehaviour
	{
		public static float LocalTime
		{
			get
			{
				return Timing.Instance.localTime;
			}
		}

		public static float DeltaTime
		{
			get
			{
				return Timing.Instance.deltaTime;
			}
		}

		public static event Action OnPreExecute;

		public static Thread MainThread { get; private set; }

		public static CoroutineHandle CurrentCoroutine
		{
			get
			{
				for (int i = 0; i < Timing.ActiveInstances.Length; i++)
				{
					if (Timing.ActiveInstances[i] != null && Timing.ActiveInstances[i].currentCoroutine.IsValid)
					{
						return Timing.ActiveInstances[i].currentCoroutine;
					}
				}
				return default(CoroutineHandle);
			}
		}

		public CoroutineHandle currentCoroutine { get; private set; }

		public static Timing Instance
		{
			get
			{
				if (Timing._instance == null || !Timing._instance.gameObject)
				{
					GameObject gameObject = GameObject.Find("Timing Controller");
					if (gameObject == null)
					{
						gameObject = new GameObject
						{
							name = "Timing Controller"
						};
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
					Timing._instance = (gameObject.GetComponent<Timing>() ?? gameObject.AddComponent<Timing>());
					Timing._instance.InitializeInstanceID();
				}
				return Timing._instance;
			}
			set
			{
				Timing._instance = value;
			}
		}

		private void OnDestroy()
		{
			if (Timing._instance == this)
			{
				Timing._instance = null;
			}
		}

		private void OnEnable()
		{
			if (Timing.MainThread == null)
			{
				Timing.MainThread = Thread.CurrentThread;
			}
			if (this._nextEditorUpdateProcessSlot > 0 || this._nextEditorSlowUpdateProcessSlot > 0)
			{
				this.OnEditorStart();
			}
			this.InitializeInstanceID();
			if (this._nextEndOfFrameProcessSlot > 0)
			{
				this.RunCoroutineSingletonOnInstance(this._EOFPumpWatcher(), "MEC_EOFPumpWatcher", SingletonBehavior.Abort);
			}
		}

		private void OnDisable()
		{
			if ((int)this._instanceID < Timing.ActiveInstances.Length)
			{
				Timing.ActiveInstances[(int)this._instanceID] = null;
			}
		}

		private void InitializeInstanceID()
		{
			if (Timing.ActiveInstances[(int)this._instanceID] == null)
			{
				if (this._instanceID == 0)
				{
					this._instanceID += 1;
				}
				while (this._instanceID <= 16)
				{
					if (this._instanceID == 16)
					{
						UnityEngine.Object.Destroy(base.gameObject);
						throw new OverflowException("You are only allowed 15 different contexts for MEC to run inside at one time.");
					}
					if (Timing.ActiveInstances[(int)this._instanceID] == null)
					{
						Timing.ActiveInstances[(int)this._instanceID] = this;
						return;
					}
					this._instanceID += 1;
				}
			}
		}

		private void Update()
		{
			if (Timing.OnPreExecute != null)
			{
				Timing.OnPreExecute();
			}
			if (this._lastSlowUpdateTime + this.TimeBetweenSlowUpdateCalls < Time.realtimeSinceStartup && this._nextSlowUpdateProcessSlot > 0)
			{
				Timing.ProcessIndex processIndex = new Timing.ProcessIndex
				{
					seg = Segment.SlowUpdate
				};
				if (this.UpdateTimeValues(processIndex.seg))
				{
					this._lastSlowUpdateProcessSlot = this._nextSlowUpdateProcessSlot;
				}
				processIndex.i = 0;
				while (processIndex.i < this._lastSlowUpdateProcessSlot)
				{
					try
					{
						if (!this.SlowUpdatePaused[processIndex.i] && !this.SlowUpdateHeld[processIndex.i] && this.SlowUpdateProcesses[processIndex.i] != null && this.localTime >= this.SlowUpdateProcesses[processIndex.i].Current)
						{
							this.currentCoroutine = this._indexToHandle[processIndex];
							if (this.ProfilerDebugAmount != DebugInfoType.None)
							{
								this._indexToHandle.ContainsKey(processIndex);
							}
							if (!this.SlowUpdateProcesses[processIndex.i].MoveNext())
							{
								if (this._indexToHandle.ContainsKey(processIndex))
								{
									this.KillCoroutinesOnInstance(this._indexToHandle[processIndex]);
								}
							}
							else if (this.SlowUpdateProcesses[processIndex.i] != null && float.IsNaN(this.SlowUpdateProcesses[processIndex.i].Current))
							{
								if (Timing.ReplacementFunction != null)
								{
									this.SlowUpdateProcesses[processIndex.i] = Timing.ReplacementFunction(this.SlowUpdateProcesses[processIndex.i], this._indexToHandle[processIndex]);
									Timing.ReplacementFunction = null;
								}
								processIndex.i--;
							}
							DebugInfoType profilerDebugAmount = this.ProfilerDebugAmount;
						}
					}
					catch (Exception ex)
					{
						Debug.LogException(ex);
						if (ex is MissingReferenceException)
						{
							Debug.LogError("This exception can probably be fixed by adding \"CancelWith(gameObject)\" when you run the coroutine.\nExample: Timing.RunCoroutine(_foo().CancelWith(gameObject), Segment.SlowUpdate);");
						}
					}
					processIndex.i++;
				}
			}
			if (this._nextRealtimeUpdateProcessSlot > 0)
			{
				Timing.ProcessIndex processIndex2 = new Timing.ProcessIndex
				{
					seg = Segment.RealtimeUpdate
				};
				if (this.UpdateTimeValues(processIndex2.seg))
				{
					this._lastRealtimeUpdateProcessSlot = this._nextRealtimeUpdateProcessSlot;
				}
				processIndex2.i = 0;
				while (processIndex2.i < this._lastRealtimeUpdateProcessSlot)
				{
					try
					{
						if (!this.RealtimeUpdatePaused[processIndex2.i] && !this.RealtimeUpdateHeld[processIndex2.i] && this.RealtimeUpdateProcesses[processIndex2.i] != null && this.localTime >= this.RealtimeUpdateProcesses[processIndex2.i].Current)
						{
							this.currentCoroutine = this._indexToHandle[processIndex2];
							if (this.ProfilerDebugAmount != DebugInfoType.None)
							{
								this._indexToHandle.ContainsKey(processIndex2);
							}
							if (!this.RealtimeUpdateProcesses[processIndex2.i].MoveNext())
							{
								if (this._indexToHandle.ContainsKey(processIndex2))
								{
									this.KillCoroutinesOnInstance(this._indexToHandle[processIndex2]);
								}
							}
							else if (this.RealtimeUpdateProcesses[processIndex2.i] != null && float.IsNaN(this.RealtimeUpdateProcesses[processIndex2.i].Current))
							{
								if (Timing.ReplacementFunction != null)
								{
									this.RealtimeUpdateProcesses[processIndex2.i] = Timing.ReplacementFunction(this.RealtimeUpdateProcesses[processIndex2.i], this._indexToHandle[processIndex2]);
									Timing.ReplacementFunction = null;
								}
								processIndex2.i--;
							}
							DebugInfoType profilerDebugAmount2 = this.ProfilerDebugAmount;
						}
					}
					catch (Exception ex2)
					{
						Debug.LogException(ex2);
						if (ex2 is MissingReferenceException)
						{
							Debug.LogError("This exception can probably be fixed by adding \"CancelWith(gameObject)\" when you run the coroutine.\nExample: Timing.RunCoroutine(_foo().CancelWith(gameObject), Segment.RealtimeUpdate);");
						}
					}
					processIndex2.i++;
				}
			}
			if (this._nextUpdateProcessSlot > 0)
			{
				Timing.ProcessIndex processIndex3 = new Timing.ProcessIndex
				{
					seg = Segment.Update
				};
				if (this.UpdateTimeValues(processIndex3.seg))
				{
					this._lastUpdateProcessSlot = this._nextUpdateProcessSlot;
				}
				processIndex3.i = 0;
				while (processIndex3.i < this._lastUpdateProcessSlot)
				{
					try
					{
						if (!this.UpdatePaused[processIndex3.i] && !this.UpdateHeld[processIndex3.i] && this.UpdateProcesses[processIndex3.i] != null && this.localTime >= this.UpdateProcesses[processIndex3.i].Current)
						{
							this.currentCoroutine = this._indexToHandle[processIndex3];
							if (this.ProfilerDebugAmount != DebugInfoType.None)
							{
								this._indexToHandle.ContainsKey(processIndex3);
							}
							if (!this.UpdateProcesses[processIndex3.i].MoveNext())
							{
								if (this._indexToHandle.ContainsKey(processIndex3))
								{
									this.KillCoroutinesOnInstance(this._indexToHandle[processIndex3]);
								}
							}
							else if (this.UpdateProcesses[processIndex3.i] != null && float.IsNaN(this.UpdateProcesses[processIndex3.i].Current))
							{
								if (Timing.ReplacementFunction != null)
								{
									this.UpdateProcesses[processIndex3.i] = Timing.ReplacementFunction(this.UpdateProcesses[processIndex3.i], this._indexToHandle[processIndex3]);
									Timing.ReplacementFunction = null;
								}
								processIndex3.i--;
							}
							DebugInfoType profilerDebugAmount3 = this.ProfilerDebugAmount;
						}
					}
					catch (Exception ex3)
					{
						Debug.LogException(ex3);
						if (ex3 is MissingReferenceException)
						{
							Debug.LogError("This exception can probably be fixed by adding \"CancelWith(gameObject)\" when you run the coroutine.\nExample: Timing.RunCoroutine(_foo().CancelWith(gameObject), Segment.Update);");
						}
					}
					processIndex3.i++;
				}
			}
			if (this.AutoTriggerManualTimeframe)
			{
				this.TriggerManualTimeframeUpdate();
			}
			else
			{
				ushort num = (ushort)(this._framesSinceUpdate + 1);
				this._framesSinceUpdate = num;
				if (num > 64)
				{
					this._framesSinceUpdate = 0;
					DebugInfoType profilerDebugAmount4 = this.ProfilerDebugAmount;
					this.RemoveUnused();
					DebugInfoType profilerDebugAmount5 = this.ProfilerDebugAmount;
				}
			}
			this.currentCoroutine = default(CoroutineHandle);
		}

		private void FixedUpdate()
		{
			if (Timing.OnPreExecute != null)
			{
				Timing.OnPreExecute();
			}
			if (this._nextFixedUpdateProcessSlot > 0)
			{
				Timing.ProcessIndex processIndex = new Timing.ProcessIndex
				{
					seg = Segment.FixedUpdate
				};
				if (this.UpdateTimeValues(processIndex.seg))
				{
					this._lastFixedUpdateProcessSlot = this._nextFixedUpdateProcessSlot;
				}
				processIndex.i = 0;
				while (processIndex.i < this._lastFixedUpdateProcessSlot)
				{
					try
					{
						if (!this.FixedUpdatePaused[processIndex.i] && !this.FixedUpdateHeld[processIndex.i] && this.FixedUpdateProcesses[processIndex.i] != null && this.localTime >= this.FixedUpdateProcesses[processIndex.i].Current)
						{
							this.currentCoroutine = this._indexToHandle[processIndex];
							if (this.ProfilerDebugAmount != DebugInfoType.None)
							{
								this._indexToHandle.ContainsKey(processIndex);
							}
							if (!this.FixedUpdateProcesses[processIndex.i].MoveNext())
							{
								if (this._indexToHandle.ContainsKey(processIndex))
								{
									this.KillCoroutinesOnInstance(this._indexToHandle[processIndex]);
								}
							}
							else if (this.FixedUpdateProcesses[processIndex.i] != null && float.IsNaN(this.FixedUpdateProcesses[processIndex.i].Current))
							{
								if (Timing.ReplacementFunction != null)
								{
									this.FixedUpdateProcesses[processIndex.i] = Timing.ReplacementFunction(this.FixedUpdateProcesses[processIndex.i], this._indexToHandle[processIndex]);
									Timing.ReplacementFunction = null;
								}
								processIndex.i--;
							}
							DebugInfoType profilerDebugAmount = this.ProfilerDebugAmount;
						}
					}
					catch (Exception ex)
					{
						Debug.LogException(ex);
						if (ex is MissingReferenceException)
						{
							Debug.LogError("This exception can probably be fixed by adding \"CancelWith(gameObject)\" when you run the coroutine.\nExample: Timing.RunCoroutine(_foo().CancelWith(gameObject), Segment.FixedUpdate);");
						}
					}
					processIndex.i++;
				}
				this.currentCoroutine = default(CoroutineHandle);
			}
		}

		private void LateUpdate()
		{
			if (Timing.OnPreExecute != null)
			{
				Timing.OnPreExecute();
			}
			if (this._nextLateUpdateProcessSlot > 0)
			{
				Timing.ProcessIndex processIndex = new Timing.ProcessIndex
				{
					seg = Segment.LateUpdate
				};
				if (this.UpdateTimeValues(processIndex.seg))
				{
					this._lastLateUpdateProcessSlot = this._nextLateUpdateProcessSlot;
				}
				processIndex.i = 0;
				while (processIndex.i < this._lastLateUpdateProcessSlot)
				{
					try
					{
						if (!this.LateUpdatePaused[processIndex.i] && !this.LateUpdateHeld[processIndex.i] && this.LateUpdateProcesses[processIndex.i] != null && this.localTime >= this.LateUpdateProcesses[processIndex.i].Current)
						{
							this.currentCoroutine = this._indexToHandle[processIndex];
							if (this.ProfilerDebugAmount != DebugInfoType.None)
							{
								this._indexToHandle.ContainsKey(processIndex);
							}
							if (!this.LateUpdateProcesses[processIndex.i].MoveNext())
							{
								if (this._indexToHandle.ContainsKey(processIndex))
								{
									this.KillCoroutinesOnInstance(this._indexToHandle[processIndex]);
								}
							}
							else if (this.LateUpdateProcesses[processIndex.i] != null && float.IsNaN(this.LateUpdateProcesses[processIndex.i].Current))
							{
								if (Timing.ReplacementFunction != null)
								{
									this.LateUpdateProcesses[processIndex.i] = Timing.ReplacementFunction(this.LateUpdateProcesses[processIndex.i], this._indexToHandle[processIndex]);
									Timing.ReplacementFunction = null;
								}
								processIndex.i--;
							}
							DebugInfoType profilerDebugAmount = this.ProfilerDebugAmount;
						}
					}
					catch (Exception ex)
					{
						Debug.LogException(ex);
						if (ex is MissingReferenceException)
						{
							Debug.LogError("This exception can probably be fixed by adding \"CancelWith(gameObject)\" when you run the coroutine.\nExample: Timing.RunCoroutine(_foo().CancelWith(gameObject), Segment.LateUpdate);");
						}
					}
					processIndex.i++;
				}
				this.currentCoroutine = default(CoroutineHandle);
			}
		}

		public void TriggerManualTimeframeUpdate()
		{
			if (Timing.OnPreExecute != null)
			{
				Timing.OnPreExecute();
			}
			if (this._nextManualTimeframeProcessSlot > 0)
			{
				Timing.ProcessIndex processIndex = new Timing.ProcessIndex
				{
					seg = Segment.ManualTimeframe
				};
				if (this.UpdateTimeValues(processIndex.seg))
				{
					this._lastManualTimeframeProcessSlot = this._nextManualTimeframeProcessSlot;
				}
				processIndex.i = 0;
				while (processIndex.i < this._lastManualTimeframeProcessSlot)
				{
					try
					{
						if (!this.ManualTimeframePaused[processIndex.i] && !this.ManualTimeframeHeld[processIndex.i] && this.ManualTimeframeProcesses[processIndex.i] != null && this.localTime >= this.ManualTimeframeProcesses[processIndex.i].Current)
						{
							this.currentCoroutine = this._indexToHandle[processIndex];
							if (this.ProfilerDebugAmount != DebugInfoType.None)
							{
								this._indexToHandle.ContainsKey(processIndex);
							}
							if (!this.ManualTimeframeProcesses[processIndex.i].MoveNext())
							{
								if (this._indexToHandle.ContainsKey(processIndex))
								{
									this.KillCoroutinesOnInstance(this._indexToHandle[processIndex]);
								}
							}
							else if (this.ManualTimeframeProcesses[processIndex.i] != null && float.IsNaN(this.ManualTimeframeProcesses[processIndex.i].Current))
							{
								if (Timing.ReplacementFunction != null)
								{
									this.ManualTimeframeProcesses[processIndex.i] = Timing.ReplacementFunction(this.ManualTimeframeProcesses[processIndex.i], this._indexToHandle[processIndex]);
									Timing.ReplacementFunction = null;
								}
								processIndex.i--;
							}
							DebugInfoType profilerDebugAmount = this.ProfilerDebugAmount;
						}
					}
					catch (Exception ex)
					{
						Debug.LogException(ex);
						if (ex is MissingReferenceException)
						{
							Debug.LogError("This exception can probably be fixed by adding \"CancelWith(gameObject)\" when you run the coroutine.\nExample: Timing.RunCoroutine(_foo().CancelWith(gameObject), Segment.ManualTimeframe);");
						}
					}
					processIndex.i++;
				}
			}
			ushort num = (ushort)(this._framesSinceUpdate + 1);
			this._framesSinceUpdate = num;
			if (num > 64)
			{
				this._framesSinceUpdate = 0;
				DebugInfoType profilerDebugAmount2 = this.ProfilerDebugAmount;
				this.RemoveUnused();
				DebugInfoType profilerDebugAmount3 = this.ProfilerDebugAmount;
			}
			this.currentCoroutine = default(CoroutineHandle);
		}

		private bool OnEditorStart()
		{
			return false;
		}

		private IEnumerator<float> _EOFPumpWatcher()
		{
			while (this._nextEndOfFrameProcessSlot > 0)
			{
				if (!this._EOFPumpRan)
				{
					base.StartCoroutine(this._EOFPump());
				}
				this._EOFPumpRan = false;
				yield return float.NegativeInfinity;
			}
			this._EOFPumpRan = false;
			yield break;
		}

		private IEnumerator _EOFPump()
		{
			while (this._nextEndOfFrameProcessSlot > 0)
			{
				yield return Timing.EofWaitObject;
				if (Timing.OnPreExecute != null)
				{
					Timing.OnPreExecute();
				}
				Timing.ProcessIndex processIndex = new Timing.ProcessIndex
				{
					seg = Segment.EndOfFrame
				};
				this._EOFPumpRan = true;
				if (this.UpdateTimeValues(processIndex.seg))
				{
					this._lastEndOfFrameProcessSlot = this._nextEndOfFrameProcessSlot;
				}
				processIndex.i = 0;
				while (processIndex.i < this._lastEndOfFrameProcessSlot)
				{
					try
					{
						if (!this.EndOfFramePaused[processIndex.i] && !this.EndOfFrameHeld[processIndex.i] && this.EndOfFrameProcesses[processIndex.i] != null && this.localTime >= this.EndOfFrameProcesses[processIndex.i].Current)
						{
							this.currentCoroutine = this._indexToHandle[processIndex];
							if (this.ProfilerDebugAmount != DebugInfoType.None)
							{
								this._indexToHandle.ContainsKey(processIndex);
							}
							if (!this.EndOfFrameProcesses[processIndex.i].MoveNext())
							{
								if (this._indexToHandle.ContainsKey(processIndex))
								{
									this.KillCoroutinesOnInstance(this._indexToHandle[processIndex]);
								}
							}
							else if (this.EndOfFrameProcesses[processIndex.i] != null && float.IsNaN(this.EndOfFrameProcesses[processIndex.i].Current))
							{
								if (Timing.ReplacementFunction != null)
								{
									this.EndOfFrameProcesses[processIndex.i] = Timing.ReplacementFunction(this.EndOfFrameProcesses[processIndex.i], this._indexToHandle[processIndex]);
									Timing.ReplacementFunction = null;
								}
								processIndex.i--;
							}
							DebugInfoType profilerDebugAmount = this.ProfilerDebugAmount;
						}
					}
					catch (Exception ex)
					{
						Debug.LogException(ex);
						if (ex is MissingReferenceException)
						{
							Debug.LogError("This exception can probably be fixed by adding \"CancelWith(gameObject)\" when you run the coroutine.\nExample: Timing.RunCoroutine(_foo().CancelWith(gameObject), Segment.EndOfFrame);");
						}
					}
					processIndex.i++;
				}
			}
			this.currentCoroutine = default(CoroutineHandle);
			yield break;
		}

		private void RemoveUnused()
		{
			foreach (KeyValuePair<CoroutineHandle, HashSet<CoroutineHandle>> keyValuePair in this._waitingTriggers)
			{
				if (keyValuePair.Value.Count == 0)
				{
					this._waitingTriggers.Remove(keyValuePair.Key);
				}
				else
				{
					if (this._handleToIndex.ContainsKey(keyValuePair.Key))
					{
						if (this.CoindexIsNull(this._handleToIndex[keyValuePair.Key]))
						{
							this.CloseWaitingProcess(keyValuePair.Key);
						}
					}
				}
			}
			Timing.ProcessIndex processIndex;
			Timing.ProcessIndex processIndex2;
			processIndex.seg = (processIndex2.seg = Segment.Update);
			processIndex.i = (processIndex2.i = 0);
			while (processIndex.i < this._nextUpdateProcessSlot)
			{
				if (this.UpdateProcesses[processIndex.i] != null)
				{
					if (processIndex.i != processIndex2.i)
					{
						this.UpdateProcesses[processIndex2.i] = this.UpdateProcesses[processIndex.i];
						this.UpdatePaused[processIndex2.i] = this.UpdatePaused[processIndex.i];
						this.UpdateHeld[processIndex2.i] = this.UpdateHeld[processIndex.i];
						if (this._indexToHandle.ContainsKey(processIndex2))
						{
							this.RemoveGraffiti(this._indexToHandle[processIndex2]);
							this._handleToIndex.Remove(this._indexToHandle[processIndex2]);
							this._indexToHandle.Remove(processIndex2);
						}
						this._handleToIndex[this._indexToHandle[processIndex]] = processIndex2;
						this._indexToHandle.Add(processIndex2, this._indexToHandle[processIndex]);
						this._indexToHandle.Remove(processIndex);
					}
					processIndex2.i++;
				}
				processIndex.i++;
			}
			processIndex.i = processIndex2.i;
			while (processIndex.i < this._nextUpdateProcessSlot)
			{
				this.UpdateProcesses[processIndex.i] = null;
				this.UpdatePaused[processIndex.i] = false;
				this.UpdateHeld[processIndex.i] = false;
				if (this._indexToHandle.ContainsKey(processIndex))
				{
					this.RemoveGraffiti(this._indexToHandle[processIndex]);
					this._handleToIndex.Remove(this._indexToHandle[processIndex]);
					this._indexToHandle.Remove(processIndex);
				}
				processIndex.i++;
			}
			this.UpdateCoroutines = (this._nextUpdateProcessSlot = processIndex2.i);
			processIndex.seg = (processIndex2.seg = Segment.FixedUpdate);
			processIndex.i = (processIndex2.i = 0);
			while (processIndex.i < this._nextFixedUpdateProcessSlot)
			{
				if (this.FixedUpdateProcesses[processIndex.i] != null)
				{
					if (processIndex.i != processIndex2.i)
					{
						this.FixedUpdateProcesses[processIndex2.i] = this.FixedUpdateProcesses[processIndex.i];
						this.FixedUpdatePaused[processIndex2.i] = this.FixedUpdatePaused[processIndex.i];
						this.FixedUpdateHeld[processIndex2.i] = this.FixedUpdateHeld[processIndex.i];
						if (this._indexToHandle.ContainsKey(processIndex2))
						{
							this.RemoveGraffiti(this._indexToHandle[processIndex2]);
							this._handleToIndex.Remove(this._indexToHandle[processIndex2]);
							this._indexToHandle.Remove(processIndex2);
						}
						this._handleToIndex[this._indexToHandle[processIndex]] = processIndex2;
						this._indexToHandle.Add(processIndex2, this._indexToHandle[processIndex]);
						this._indexToHandle.Remove(processIndex);
					}
					processIndex2.i++;
				}
				processIndex.i++;
			}
			processIndex.i = processIndex2.i;
			while (processIndex.i < this._nextFixedUpdateProcessSlot)
			{
				this.FixedUpdateProcesses[processIndex.i] = null;
				this.FixedUpdatePaused[processIndex.i] = false;
				this.FixedUpdateHeld[processIndex.i] = false;
				if (this._indexToHandle.ContainsKey(processIndex))
				{
					this.RemoveGraffiti(this._indexToHandle[processIndex]);
					this._handleToIndex.Remove(this._indexToHandle[processIndex]);
					this._indexToHandle.Remove(processIndex);
				}
				processIndex.i++;
			}
			this.FixedUpdateCoroutines = (this._nextFixedUpdateProcessSlot = processIndex2.i);
			processIndex.seg = (processIndex2.seg = Segment.LateUpdate);
			processIndex.i = (processIndex2.i = 0);
			while (processIndex.i < this._nextLateUpdateProcessSlot)
			{
				if (this.LateUpdateProcesses[processIndex.i] != null)
				{
					if (processIndex.i != processIndex2.i)
					{
						this.LateUpdateProcesses[processIndex2.i] = this.LateUpdateProcesses[processIndex.i];
						this.LateUpdatePaused[processIndex2.i] = this.LateUpdatePaused[processIndex.i];
						this.LateUpdateHeld[processIndex2.i] = this.LateUpdateHeld[processIndex.i];
						if (this._indexToHandle.ContainsKey(processIndex2))
						{
							this.RemoveGraffiti(this._indexToHandle[processIndex2]);
							this._handleToIndex.Remove(this._indexToHandle[processIndex2]);
							this._indexToHandle.Remove(processIndex2);
						}
						this._handleToIndex[this._indexToHandle[processIndex]] = processIndex2;
						this._indexToHandle.Add(processIndex2, this._indexToHandle[processIndex]);
						this._indexToHandle.Remove(processIndex);
					}
					processIndex2.i++;
				}
				processIndex.i++;
			}
			processIndex.i = processIndex2.i;
			while (processIndex.i < this._nextLateUpdateProcessSlot)
			{
				this.LateUpdateProcesses[processIndex.i] = null;
				this.LateUpdatePaused[processIndex.i] = false;
				this.LateUpdateHeld[processIndex.i] = false;
				if (this._indexToHandle.ContainsKey(processIndex))
				{
					this.RemoveGraffiti(this._indexToHandle[processIndex]);
					this._handleToIndex.Remove(this._indexToHandle[processIndex]);
					this._indexToHandle.Remove(processIndex);
				}
				processIndex.i++;
			}
			this.LateUpdateCoroutines = (this._nextLateUpdateProcessSlot = processIndex2.i);
			processIndex.seg = (processIndex2.seg = Segment.SlowUpdate);
			processIndex.i = (processIndex2.i = 0);
			while (processIndex.i < this._nextSlowUpdateProcessSlot)
			{
				if (this.SlowUpdateProcesses[processIndex.i] != null)
				{
					if (processIndex.i != processIndex2.i)
					{
						this.SlowUpdateProcesses[processIndex2.i] = this.SlowUpdateProcesses[processIndex.i];
						this.SlowUpdatePaused[processIndex2.i] = this.SlowUpdatePaused[processIndex.i];
						this.SlowUpdateHeld[processIndex2.i] = this.SlowUpdateHeld[processIndex.i];
						if (this._indexToHandle.ContainsKey(processIndex2))
						{
							this.RemoveGraffiti(this._indexToHandle[processIndex2]);
							this._handleToIndex.Remove(this._indexToHandle[processIndex2]);
							this._indexToHandle.Remove(processIndex2);
						}
						this._handleToIndex[this._indexToHandle[processIndex]] = processIndex2;
						this._indexToHandle.Add(processIndex2, this._indexToHandle[processIndex]);
						this._indexToHandle.Remove(processIndex);
					}
					processIndex2.i++;
				}
				processIndex.i++;
			}
			processIndex.i = processIndex2.i;
			while (processIndex.i < this._nextSlowUpdateProcessSlot)
			{
				this.SlowUpdateProcesses[processIndex.i] = null;
				this.SlowUpdatePaused[processIndex.i] = false;
				this.SlowUpdateHeld[processIndex.i] = false;
				if (this._indexToHandle.ContainsKey(processIndex))
				{
					this.RemoveGraffiti(this._indexToHandle[processIndex]);
					this._handleToIndex.Remove(this._indexToHandle[processIndex]);
					this._indexToHandle.Remove(processIndex);
				}
				processIndex.i++;
			}
			this.SlowUpdateCoroutines = (this._nextSlowUpdateProcessSlot = processIndex2.i);
			processIndex.seg = (processIndex2.seg = Segment.RealtimeUpdate);
			processIndex.i = (processIndex2.i = 0);
			while (processIndex.i < this._nextRealtimeUpdateProcessSlot)
			{
				if (this.RealtimeUpdateProcesses[processIndex.i] != null)
				{
					if (processIndex.i != processIndex2.i)
					{
						this.RealtimeUpdateProcesses[processIndex2.i] = this.RealtimeUpdateProcesses[processIndex.i];
						this.RealtimeUpdatePaused[processIndex2.i] = this.RealtimeUpdatePaused[processIndex.i];
						this.RealtimeUpdateHeld[processIndex2.i] = this.RealtimeUpdateHeld[processIndex.i];
						if (this._indexToHandle.ContainsKey(processIndex2))
						{
							this.RemoveGraffiti(this._indexToHandle[processIndex2]);
							this._handleToIndex.Remove(this._indexToHandle[processIndex2]);
							this._indexToHandle.Remove(processIndex2);
						}
						this._handleToIndex[this._indexToHandle[processIndex]] = processIndex2;
						this._indexToHandle.Add(processIndex2, this._indexToHandle[processIndex]);
						this._indexToHandle.Remove(processIndex);
					}
					processIndex2.i++;
				}
				processIndex.i++;
			}
			processIndex.i = processIndex2.i;
			while (processIndex.i < this._nextRealtimeUpdateProcessSlot)
			{
				this.RealtimeUpdateProcesses[processIndex.i] = null;
				this.RealtimeUpdatePaused[processIndex.i] = false;
				this.RealtimeUpdateHeld[processIndex.i] = false;
				if (this._indexToHandle.ContainsKey(processIndex))
				{
					this.RemoveGraffiti(this._indexToHandle[processIndex]);
					this._handleToIndex.Remove(this._indexToHandle[processIndex]);
					this._indexToHandle.Remove(processIndex);
				}
				processIndex.i++;
			}
			this.RealtimeUpdateCoroutines = (this._nextRealtimeUpdateProcessSlot = processIndex2.i);
			processIndex.seg = (processIndex2.seg = Segment.EndOfFrame);
			processIndex.i = (processIndex2.i = 0);
			while (processIndex.i < this._nextEndOfFrameProcessSlot)
			{
				if (this.EndOfFrameProcesses[processIndex.i] != null)
				{
					if (processIndex.i != processIndex2.i)
					{
						this.EndOfFrameProcesses[processIndex2.i] = this.EndOfFrameProcesses[processIndex.i];
						this.EndOfFramePaused[processIndex2.i] = this.EndOfFramePaused[processIndex.i];
						this.EndOfFrameHeld[processIndex2.i] = this.EndOfFrameHeld[processIndex.i];
						if (this._indexToHandle.ContainsKey(processIndex2))
						{
							this.RemoveGraffiti(this._indexToHandle[processIndex2]);
							this._handleToIndex.Remove(this._indexToHandle[processIndex2]);
							this._indexToHandle.Remove(processIndex2);
						}
						this._handleToIndex[this._indexToHandle[processIndex]] = processIndex2;
						this._indexToHandle.Add(processIndex2, this._indexToHandle[processIndex]);
						this._indexToHandle.Remove(processIndex);
					}
					processIndex2.i++;
				}
				processIndex.i++;
			}
			processIndex.i = processIndex2.i;
			while (processIndex.i < this._nextEndOfFrameProcessSlot)
			{
				this.EndOfFrameProcesses[processIndex.i] = null;
				this.EndOfFramePaused[processIndex.i] = false;
				this.EndOfFrameHeld[processIndex.i] = false;
				if (this._indexToHandle.ContainsKey(processIndex))
				{
					this.RemoveGraffiti(this._indexToHandle[processIndex]);
					this._handleToIndex.Remove(this._indexToHandle[processIndex]);
					this._indexToHandle.Remove(processIndex);
				}
				processIndex.i++;
			}
			this.EndOfFrameCoroutines = (this._nextEndOfFrameProcessSlot = processIndex2.i);
			processIndex.seg = (processIndex2.seg = Segment.ManualTimeframe);
			processIndex.i = (processIndex2.i = 0);
			while (processIndex.i < this._nextManualTimeframeProcessSlot)
			{
				if (this.ManualTimeframeProcesses[processIndex.i] != null)
				{
					if (processIndex.i != processIndex2.i)
					{
						this.ManualTimeframeProcesses[processIndex2.i] = this.ManualTimeframeProcesses[processIndex.i];
						this.ManualTimeframePaused[processIndex2.i] = this.ManualTimeframePaused[processIndex.i];
						this.ManualTimeframeHeld[processIndex2.i] = this.ManualTimeframeHeld[processIndex.i];
						if (this._indexToHandle.ContainsKey(processIndex2))
						{
							this.RemoveGraffiti(this._indexToHandle[processIndex2]);
							this._handleToIndex.Remove(this._indexToHandle[processIndex2]);
							this._indexToHandle.Remove(processIndex2);
						}
						this._handleToIndex[this._indexToHandle[processIndex]] = processIndex2;
						this._indexToHandle.Add(processIndex2, this._indexToHandle[processIndex]);
						this._indexToHandle.Remove(processIndex);
					}
					processIndex2.i++;
				}
				processIndex.i++;
			}
			processIndex.i = processIndex2.i;
			while (processIndex.i < this._nextManualTimeframeProcessSlot)
			{
				this.ManualTimeframeProcesses[processIndex.i] = null;
				this.ManualTimeframePaused[processIndex.i] = false;
				this.ManualTimeframeHeld[processIndex.i] = false;
				if (this._indexToHandle.ContainsKey(processIndex))
				{
					this.RemoveGraffiti(this._indexToHandle[processIndex]);
					this._handleToIndex.Remove(this._indexToHandle[processIndex]);
					this._indexToHandle.Remove(processIndex);
				}
				processIndex.i++;
			}
			this.ManualTimeframeCoroutines = (this._nextManualTimeframeProcessSlot = processIndex2.i);
		}

		private void EditorRemoveUnused()
		{
			Dictionary<CoroutineHandle, HashSet<CoroutineHandle>>.Enumerator enumerator = this._waitingTriggers.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Dictionary<CoroutineHandle, Timing.ProcessIndex> handleToIndex = this._handleToIndex;
				KeyValuePair<CoroutineHandle, HashSet<CoroutineHandle>> keyValuePair = enumerator.Current;
				if (handleToIndex.ContainsKey(keyValuePair.Key))
				{
					Dictionary<CoroutineHandle, Timing.ProcessIndex> handleToIndex2 = this._handleToIndex;
					keyValuePair = enumerator.Current;
					if (this.CoindexIsNull(handleToIndex2[keyValuePair.Key]))
					{
						keyValuePair = enumerator.Current;
						this.CloseWaitingProcess(keyValuePair.Key);
						enumerator = this._waitingTriggers.GetEnumerator();
					}
				}
			}
			Timing.ProcessIndex processIndex;
			Timing.ProcessIndex processIndex2;
			processIndex.seg = (processIndex2.seg = Segment.EditorUpdate);
			processIndex.i = (processIndex2.i = 0);
			while (processIndex.i < this._nextEditorUpdateProcessSlot)
			{
				if (this.EditorUpdateProcesses[processIndex.i] != null)
				{
					if (processIndex.i != processIndex2.i)
					{
						this.EditorUpdateProcesses[processIndex2.i] = this.EditorUpdateProcesses[processIndex.i];
						this.EditorUpdatePaused[processIndex2.i] = this.EditorUpdatePaused[processIndex.i];
						this.EditorUpdateHeld[processIndex2.i] = this.EditorUpdateHeld[processIndex.i];
						if (this._indexToHandle.ContainsKey(processIndex2))
						{
							this.RemoveGraffiti(this._indexToHandle[processIndex2]);
							this._handleToIndex.Remove(this._indexToHandle[processIndex2]);
							this._indexToHandle.Remove(processIndex2);
						}
						this._handleToIndex[this._indexToHandle[processIndex]] = processIndex2;
						this._indexToHandle.Add(processIndex2, this._indexToHandle[processIndex]);
						this._indexToHandle.Remove(processIndex);
					}
					processIndex2.i++;
				}
				processIndex.i++;
			}
			processIndex.i = processIndex2.i;
			while (processIndex.i < this._nextEditorUpdateProcessSlot)
			{
				this.EditorUpdateProcesses[processIndex.i] = null;
				this.EditorUpdatePaused[processIndex.i] = false;
				this.EditorUpdateHeld[processIndex.i] = false;
				if (this._indexToHandle.ContainsKey(processIndex))
				{
					this.RemoveGraffiti(this._indexToHandle[processIndex]);
					this._handleToIndex.Remove(this._indexToHandle[processIndex]);
					this._indexToHandle.Remove(processIndex);
				}
				processIndex.i++;
			}
			this.EditorUpdateCoroutines = (this._nextEditorUpdateProcessSlot = processIndex2.i);
			processIndex.seg = (processIndex2.seg = Segment.EditorSlowUpdate);
			processIndex.i = (processIndex2.i = 0);
			while (processIndex.i < this._nextEditorSlowUpdateProcessSlot)
			{
				if (this.EditorSlowUpdateProcesses[processIndex.i] != null)
				{
					if (processIndex.i != processIndex2.i)
					{
						this.EditorSlowUpdateProcesses[processIndex2.i] = this.EditorSlowUpdateProcesses[processIndex.i];
						this.EditorUpdatePaused[processIndex2.i] = this.EditorUpdatePaused[processIndex.i];
						this.EditorUpdateHeld[processIndex2.i] = this.EditorUpdateHeld[processIndex.i];
						if (this._indexToHandle.ContainsKey(processIndex2))
						{
							this.RemoveGraffiti(this._indexToHandle[processIndex2]);
							this._handleToIndex.Remove(this._indexToHandle[processIndex2]);
							this._indexToHandle.Remove(processIndex2);
						}
						this._handleToIndex[this._indexToHandle[processIndex]] = processIndex2;
						this._indexToHandle.Add(processIndex2, this._indexToHandle[processIndex]);
						this._indexToHandle.Remove(processIndex);
					}
					processIndex2.i++;
				}
				processIndex.i++;
			}
			processIndex.i = processIndex2.i;
			while (processIndex.i < this._nextEditorSlowUpdateProcessSlot)
			{
				this.EditorSlowUpdateProcesses[processIndex.i] = null;
				this.EditorSlowUpdatePaused[processIndex.i] = false;
				this.EditorSlowUpdateHeld[processIndex.i] = false;
				if (this._indexToHandle.ContainsKey(processIndex))
				{
					this.RemoveGraffiti(this._indexToHandle[processIndex]);
					this._handleToIndex.Remove(this._indexToHandle[processIndex]);
					this._indexToHandle.Remove(processIndex);
				}
				processIndex.i++;
			}
			this.EditorSlowUpdateCoroutines = (this._nextEditorSlowUpdateProcessSlot = processIndex2.i);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, 0, false, null, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, GameObject gameObj)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, (gameObj == null) ? 0 : gameObj.GetInstanceID(), gameObj != null, null, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, int layer)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, layer, true, null, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, string tag)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, 0, false, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, GameObject gameObj, string tag)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, (gameObj == null) ? 0 : gameObj.GetInstanceID(), gameObj != null, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, int layer, string tag)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, layer, true, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, Segment segment)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, segment, 0, false, null, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, Segment segment, GameObject gameObj)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, segment, (gameObj == null) ? 0 : gameObj.GetInstanceID(), gameObj != null, null, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, Segment segment, int layer)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, segment, layer, true, null, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, Segment segment, string tag)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, segment, 0, false, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, Segment segment, GameObject gameObj, string tag)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, segment, (gameObj == null) ? 0 : gameObj.GetInstanceID(), gameObj != null, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, Segment segment, int layer, string tag)
		{
			if (coroutine != null)
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, segment, layer, true, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, Segment.Update, 0, false, null, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, GameObject gameObj)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, Segment.Update, (gameObj == null) ? 0 : gameObj.GetInstanceID(), gameObj != null, null, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, int layer)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, Segment.Update, layer, true, null, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, string tag)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, Segment.Update, 0, false, tag, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, GameObject gameObj, string tag)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, Segment.Update, (gameObj == null) ? 0 : gameObj.GetInstanceID(), gameObj != null, tag, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, int layer, string tag)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, Segment.Update, layer, true, tag, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, Segment segment)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, segment, 0, false, null, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, Segment segment, GameObject gameObj)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, segment, (gameObj == null) ? 0 : gameObj.GetInstanceID(), gameObj != null, null, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, Segment segment, int layer)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, segment, layer, true, null, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, Segment segment, string tag)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, segment, 0, false, tag, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, Segment segment, GameObject gameObj, string tag)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, segment, (gameObj == null) ? 0 : gameObj.GetInstanceID(), gameObj != null, tag, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, Segment segment, int layer, string tag)
		{
			if (coroutine != null)
			{
				return this.RunCoroutineInternal(coroutine, segment, layer, true, tag, new CoroutineHandle(this._instanceID), true);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, CoroutineHandle handle, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				Timing.KillCoroutines(new CoroutineHandle[]
				{
					handle
				});
			}
			else if (Timing.IsRunning(handle))
			{
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
						return handle;
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, 0, false, null, new CoroutineHandle(Timing.Instance._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, handle, false);
							return coroutineHandle;
						}
					case SingletonBehavior.AbortAndUnpause:
						Timing.ResumeCoroutines(new CoroutineHandle[]
						{
						handle
						});
						return handle;
				}
			}
			return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, 0, false, null, new CoroutineHandle(Timing.Instance._instanceID), true);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, GameObject gameObj, SingletonBehavior behaviorOnCollision)
		{
			if (!(gameObj == null))
			{
				return Timing.RunCoroutineSingleton(coroutine, gameObj.GetInstanceID(), behaviorOnCollision);
			}
			return Timing.RunCoroutine(coroutine);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, int layer, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				Timing.KillCoroutines(layer);
			}
			else if (Timing.Instance._layeredProcesses.ContainsKey(layer))
			{
				if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
				{
					Timing._instance.ResumeCoroutinesOnInstance(Timing._instance._layeredProcesses[layer]);
				}
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
					case SingletonBehavior.AbortAndUnpause:
						{
							HashSet<CoroutineHandle>.Enumerator enumerator = Timing.Instance._layeredProcesses[layer].GetEnumerator();
							while (enumerator.MoveNext())
							{
								if (Timing.IsRunning(enumerator.Current))
								{
									return enumerator.Current;
								}
							}
							break;
						}
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, layer, true, null, new CoroutineHandle(Timing.Instance._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, Timing._instance._layeredProcesses[layer], false);
							return coroutineHandle;
						}
				}
			}
			return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, layer, true, null, new CoroutineHandle(Timing.Instance._instanceID), true);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				Timing.KillCoroutines(tag);
			}
			else if (Timing.Instance._taggedProcesses.ContainsKey(tag))
			{
				if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
				{
					Timing._instance.ResumeCoroutinesOnInstance(Timing._instance._taggedProcesses[tag]);
				}
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
					case SingletonBehavior.AbortAndUnpause:
						{
							HashSet<CoroutineHandle>.Enumerator enumerator = Timing.Instance._taggedProcesses[tag].GetEnumerator();
							while (enumerator.MoveNext())
							{
								if (Timing.IsRunning(enumerator.Current))
								{
									return enumerator.Current;
								}
							}
							break;
						}
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, 0, false, tag, new CoroutineHandle(Timing.Instance._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, Timing._instance._taggedProcesses[tag], false);
							return coroutineHandle;
						}
				}
			}
			return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, 0, false, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, GameObject gameObj, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (!(gameObj == null))
			{
				return Timing.RunCoroutineSingleton(coroutine, gameObj.GetInstanceID(), tag, behaviorOnCollision);
			}
			return Timing.RunCoroutineSingleton(coroutine, tag, behaviorOnCollision);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, int layer, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				Timing.KillCoroutines(layer, tag);
				return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, layer, true, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			if (!Timing.Instance._taggedProcesses.ContainsKey(tag) || !Timing.Instance._layeredProcesses.ContainsKey(layer))
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, layer, true, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
			{
				Timing.ResumeCoroutines(layer, tag);
			}
			if (behaviorOnCollision == SingletonBehavior.Abort || behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
			{
				HashSet<CoroutineHandle>.Enumerator enumerator = Timing.Instance._taggedProcesses[tag].GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (Timing._instance._processLayers.ContainsKey(enumerator.Current) && Timing._instance._processLayers[enumerator.Current] == layer)
					{
						return enumerator.Current;
					}
				}
			}
			if (behaviorOnCollision == SingletonBehavior.Wait)
			{
				List<CoroutineHandle> list = new List<CoroutineHandle>();
				HashSet<CoroutineHandle>.Enumerator enumerator2 = Timing.Instance._taggedProcesses[tag].GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (Timing.Instance._processLayers.ContainsKey(enumerator2.Current) && Timing.Instance._processLayers[enumerator2.Current] == layer)
					{
						list.Add(enumerator2.Current);
					}
				}
				if (list.Count > 0)
				{
					CoroutineHandle coroutineHandle = Timing._instance.RunCoroutineInternal(coroutine, Segment.Update, layer, true, tag, new CoroutineHandle(Timing._instance._instanceID), false);
					Timing.WaitForOtherHandles(coroutineHandle, list, false);
					return coroutineHandle;
				}
			}
			return Timing.Instance.RunCoroutineInternal(coroutine, Segment.Update, layer, true, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, CoroutineHandle handle, Segment segment, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				Timing.KillCoroutines(new CoroutineHandle[]
				{
					handle
				});
			}
			else if (Timing.IsRunning(handle))
			{
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
						return handle;
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = Timing.Instance.RunCoroutineInternal(coroutine, segment, 0, false, null, new CoroutineHandle(Timing.Instance._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, handle, false);
							return coroutineHandle;
						}
					case SingletonBehavior.AbortAndUnpause:
						Timing.ResumeCoroutines(new CoroutineHandle[]
						{
						handle
						});
						return handle;
				}
			}
			return Timing.Instance.RunCoroutineInternal(coroutine, segment, 0, false, null, new CoroutineHandle(Timing.Instance._instanceID), true);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, Segment segment, GameObject gameObj, SingletonBehavior behaviorOnCollision)
		{
			if (!(gameObj == null))
			{
				return Timing.RunCoroutineSingleton(coroutine, segment, gameObj.GetInstanceID(), behaviorOnCollision);
			}
			return Timing.RunCoroutine(coroutine, segment);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, Segment segment, int layer, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				Timing.KillCoroutines(layer);
			}
			else if (Timing.Instance._layeredProcesses.ContainsKey(layer))
			{
				if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
				{
					Timing._instance.ResumeCoroutinesOnInstance(Timing._instance._layeredProcesses[layer]);
				}
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
					case SingletonBehavior.AbortAndUnpause:
						{
							HashSet<CoroutineHandle>.Enumerator enumerator = Timing.Instance._layeredProcesses[layer].GetEnumerator();
							while (enumerator.MoveNext())
							{
								if (Timing.IsRunning(enumerator.Current))
								{
									return enumerator.Current;
								}
							}
							break;
						}
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = Timing.Instance.RunCoroutineInternal(coroutine, segment, layer, true, null, new CoroutineHandle(Timing.Instance._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, Timing._instance._layeredProcesses[layer], false);
							return coroutineHandle;
						}
				}
			}
			return Timing.Instance.RunCoroutineInternal(coroutine, segment, layer, true, null, new CoroutineHandle(Timing.Instance._instanceID), true);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, Segment segment, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				Timing.KillCoroutines(tag);
			}
			else if (Timing.Instance._taggedProcesses.ContainsKey(tag))
			{
				if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
				{
					Timing._instance.ResumeCoroutinesOnInstance(Timing._instance._taggedProcesses[tag]);
				}
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
					case SingletonBehavior.AbortAndUnpause:
						{
							HashSet<CoroutineHandle>.Enumerator enumerator = Timing.Instance._taggedProcesses[tag].GetEnumerator();
							while (enumerator.MoveNext())
							{
								if (Timing.IsRunning(enumerator.Current))
								{
									return enumerator.Current;
								}
							}
							break;
						}
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = Timing.Instance.RunCoroutineInternal(coroutine, segment, 0, false, tag, new CoroutineHandle(Timing.Instance._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, Timing._instance._taggedProcesses[tag], false);
							return coroutineHandle;
						}
				}
			}
			return Timing.Instance.RunCoroutineInternal(coroutine, segment, 0, false, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, Segment segment, GameObject gameObj, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (!(gameObj == null))
			{
				return Timing.RunCoroutineSingleton(coroutine, segment, gameObj.GetInstanceID(), tag, behaviorOnCollision);
			}
			return Timing.RunCoroutineSingleton(coroutine, segment, tag, behaviorOnCollision);
		}

		public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, Segment segment, int layer, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				Timing.KillCoroutines(layer, tag);
				return Timing.Instance.RunCoroutineInternal(coroutine, segment, layer, true, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			if (!Timing.Instance._taggedProcesses.ContainsKey(tag) || !Timing.Instance._layeredProcesses.ContainsKey(layer))
			{
				return Timing.Instance.RunCoroutineInternal(coroutine, segment, layer, true, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
			}
			if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
			{
				Timing.ResumeCoroutines(layer, tag);
			}
			if (behaviorOnCollision == SingletonBehavior.Abort || behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
			{
				HashSet<CoroutineHandle>.Enumerator enumerator = Timing.Instance._taggedProcesses[tag].GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (Timing._instance._processLayers.ContainsKey(enumerator.Current) && Timing._instance._processLayers[enumerator.Current] == layer)
					{
						return enumerator.Current;
					}
				}
			}
			else if (behaviorOnCollision == SingletonBehavior.Wait)
			{
				List<CoroutineHandle> list = new List<CoroutineHandle>();
				HashSet<CoroutineHandle>.Enumerator enumerator2 = Timing.Instance._taggedProcesses[tag].GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (Timing._instance._processLayers.ContainsKey(enumerator2.Current) && Timing._instance._processLayers[enumerator2.Current] == layer)
					{
						list.Add(enumerator2.Current);
					}
				}
				if (list.Count > 0)
				{
					CoroutineHandle coroutineHandle = Timing._instance.RunCoroutineInternal(coroutine, segment, layer, true, tag, new CoroutineHandle(Timing._instance._instanceID), false);
					Timing.WaitForOtherHandles(coroutineHandle, list, false);
					return coroutineHandle;
				}
			}
			return Timing.Instance.RunCoroutineInternal(coroutine, segment, layer, true, tag, new CoroutineHandle(Timing.Instance._instanceID), true);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, CoroutineHandle handle, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				this.KillCoroutinesOnInstance(handle);
			}
			else if (this._handleToIndex.ContainsKey(handle) && !this.CoindexIsNull(this._handleToIndex[handle]))
			{
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
						return handle;
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = this.RunCoroutineInternal(coroutine, Segment.Update, 0, false, null, new CoroutineHandle(this._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, handle, false);
							return coroutineHandle;
						}
					case SingletonBehavior.AbortAndUnpause:
						this.ResumeCoroutinesOnInstance(handle);
						return handle;
				}
			}
			return this.RunCoroutineInternal(coroutine, Segment.Update, 0, false, null, new CoroutineHandle(this._instanceID), true);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, GameObject gameObj, SingletonBehavior behaviorOnCollision)
		{
			if (!(gameObj == null))
			{
				return this.RunCoroutineSingletonOnInstance(coroutine, gameObj.GetInstanceID(), behaviorOnCollision);
			}
			return this.RunCoroutineOnInstance(coroutine);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, int layer, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				this.KillCoroutinesOnInstance(layer);
			}
			else if (this._layeredProcesses.ContainsKey(layer))
			{
				if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
				{
					this.ResumeCoroutinesOnInstance(this._layeredProcesses[layer]);
				}
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
					case SingletonBehavior.AbortAndUnpause:
						{
							HashSet<CoroutineHandle>.Enumerator enumerator = this._layeredProcesses[layer].GetEnumerator();
							while (enumerator.MoveNext())
							{
								if (Timing.IsRunning(enumerator.Current))
								{
									return enumerator.Current;
								}
							}
							break;
						}
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = this.RunCoroutineInternal(coroutine, Segment.Update, layer, true, null, new CoroutineHandle(this._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, this._layeredProcesses[layer], false);
							return coroutineHandle;
						}
				}
			}
			return this.RunCoroutineInternal(coroutine, Segment.Update, layer, true, null, new CoroutineHandle(this._instanceID), true);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				this.KillCoroutinesOnInstance(tag);
			}
			else if (this._taggedProcesses.ContainsKey(tag))
			{
				if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
				{
					this.ResumeCoroutinesOnInstance(this._taggedProcesses[tag]);
				}
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
					case SingletonBehavior.AbortAndUnpause:
						{
							HashSet<CoroutineHandle>.Enumerator enumerator = this._taggedProcesses[tag].GetEnumerator();
							while (enumerator.MoveNext())
							{
								if (Timing.IsRunning(enumerator.Current))
								{
									return enumerator.Current;
								}
							}
							break;
						}
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = this.RunCoroutineInternal(coroutine, Segment.Update, 0, false, tag, new CoroutineHandle(this._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, this._taggedProcesses[tag], false);
							return coroutineHandle;
						}
				}
			}
			return this.RunCoroutineInternal(coroutine, Segment.Update, 0, false, tag, new CoroutineHandle(this._instanceID), true);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, GameObject gameObj, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (!(gameObj == null))
			{
				return this.RunCoroutineSingletonOnInstance(coroutine, gameObj.GetInstanceID(), tag, behaviorOnCollision);
			}
			return this.RunCoroutineSingletonOnInstance(coroutine, tag, behaviorOnCollision);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, int layer, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				this.KillCoroutinesOnInstance(layer, tag);
				return this.RunCoroutineInternal(coroutine, Segment.Update, layer, true, tag, new CoroutineHandle(this._instanceID), true);
			}
			if (!this._taggedProcesses.ContainsKey(tag) || !this._layeredProcesses.ContainsKey(layer))
			{
				return this.RunCoroutineInternal(coroutine, Segment.Update, layer, true, tag, new CoroutineHandle(this._instanceID), true);
			}
			if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
			{
				this.ResumeCoroutinesOnInstance(layer, tag);
			}
			if (behaviorOnCollision == SingletonBehavior.Abort || behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
			{
				HashSet<CoroutineHandle>.Enumerator enumerator = this._taggedProcesses[tag].GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (this._processLayers.ContainsKey(enumerator.Current) && this._processLayers[enumerator.Current] == layer)
					{
						return enumerator.Current;
					}
				}
			}
			if (behaviorOnCollision == SingletonBehavior.Wait)
			{
				List<CoroutineHandle> list = new List<CoroutineHandle>();
				HashSet<CoroutineHandle>.Enumerator enumerator2 = this._taggedProcesses[tag].GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (this._processLayers.ContainsKey(enumerator2.Current) && this._processLayers[enumerator2.Current] == layer)
					{
						list.Add(enumerator2.Current);
					}
				}
				if (list.Count > 0)
				{
					CoroutineHandle coroutineHandle = this.RunCoroutineInternal(coroutine, Segment.Update, layer, true, tag, new CoroutineHandle(this._instanceID), false);
					Timing.WaitForOtherHandles(coroutineHandle, list, false);
					return coroutineHandle;
				}
			}
			return this.RunCoroutineInternal(coroutine, Segment.Update, layer, true, tag, new CoroutineHandle(this._instanceID), true);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, Segment segment, GameObject gameObj, SingletonBehavior behaviorOnCollision)
		{
			if (!(gameObj == null))
			{
				return this.RunCoroutineSingletonOnInstance(coroutine, segment, gameObj.GetInstanceID(), behaviorOnCollision);
			}
			return this.RunCoroutineOnInstance(coroutine, segment);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, Segment segment, int layer, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				this.KillCoroutinesOnInstance(layer);
			}
			else if (this._layeredProcesses.ContainsKey(layer))
			{
				if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
				{
					this.ResumeCoroutinesOnInstance(this._layeredProcesses[layer]);
				}
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
					case SingletonBehavior.AbortAndUnpause:
						{
							HashSet<CoroutineHandle>.Enumerator enumerator = this._layeredProcesses[layer].GetEnumerator();
							while (enumerator.MoveNext())
							{
								if (Timing.IsRunning(enumerator.Current))
								{
									return enumerator.Current;
								}
							}
							break;
						}
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = this.RunCoroutineInternal(coroutine, segment, layer, true, null, new CoroutineHandle(this._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, this._layeredProcesses[layer], false);
							return coroutineHandle;
						}
				}
			}
			return this.RunCoroutineInternal(coroutine, segment, layer, true, null, new CoroutineHandle(this._instanceID), true);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, Segment segment, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				this.KillCoroutinesOnInstance(tag);
			}
			else if (this._taggedProcesses.ContainsKey(tag))
			{
				if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
				{
					this.ResumeCoroutinesOnInstance(this._taggedProcesses[tag]);
				}
				switch (behaviorOnCollision)
				{
					case SingletonBehavior.Abort:
					case SingletonBehavior.AbortAndUnpause:
						{
							HashSet<CoroutineHandle>.Enumerator enumerator = this._taggedProcesses[tag].GetEnumerator();
							while (enumerator.MoveNext())
							{
								if (Timing.IsRunning(enumerator.Current))
								{
									return enumerator.Current;
								}
							}
							break;
						}
					case SingletonBehavior.Wait:
						{
							CoroutineHandle coroutineHandle = this.RunCoroutineInternal(coroutine, segment, 0, false, tag, new CoroutineHandle(this._instanceID), false);
							Timing.WaitForOtherHandles(coroutineHandle, this._taggedProcesses[tag], false);
							return coroutineHandle;
						}
				}
			}
			return this.RunCoroutineInternal(coroutine, segment, 0, false, tag, new CoroutineHandle(this._instanceID), true);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, Segment segment, GameObject gameObj, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (!(gameObj == null))
			{
				return this.RunCoroutineSingletonOnInstance(coroutine, segment, gameObj.GetInstanceID(), tag, behaviorOnCollision);
			}
			return this.RunCoroutineSingletonOnInstance(coroutine, segment, tag, behaviorOnCollision);
		}

		public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, Segment segment, int layer, string tag, SingletonBehavior behaviorOnCollision)
		{
			if (coroutine == null)
			{
				return default(CoroutineHandle);
			}
			if (behaviorOnCollision == SingletonBehavior.Overwrite)
			{
				this.KillCoroutinesOnInstance(layer, tag);
				return this.RunCoroutineInternal(coroutine, segment, layer, true, tag, new CoroutineHandle(this._instanceID), true);
			}
			if (!this._taggedProcesses.ContainsKey(tag) || !this._layeredProcesses.ContainsKey(layer))
			{
				return this.RunCoroutineInternal(coroutine, segment, layer, true, tag, new CoroutineHandle(this._instanceID), true);
			}
			if (behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
			{
				this.ResumeCoroutinesOnInstance(layer, tag);
			}
			if (behaviorOnCollision == SingletonBehavior.Abort || behaviorOnCollision == SingletonBehavior.AbortAndUnpause)
			{
				HashSet<CoroutineHandle>.Enumerator enumerator = this._taggedProcesses[tag].GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (this._processLayers.ContainsKey(enumerator.Current) && this._processLayers[enumerator.Current] == layer)
					{
						return enumerator.Current;
					}
				}
			}
			else if (behaviorOnCollision == SingletonBehavior.Wait)
			{
				List<CoroutineHandle> list = new List<CoroutineHandle>();
				HashSet<CoroutineHandle>.Enumerator enumerator2 = this._taggedProcesses[tag].GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (this._processLayers.ContainsKey(enumerator2.Current) && this._processLayers[enumerator2.Current] == layer)
					{
						list.Add(enumerator2.Current);
					}
				}
				if (list.Count > 0)
				{
					CoroutineHandle coroutineHandle = this.RunCoroutineInternal(coroutine, segment, layer, true, tag, new CoroutineHandle(this._instanceID), false);
					Timing.WaitForOtherHandles(coroutineHandle, list, false);
					return coroutineHandle;
				}
			}
			return this.RunCoroutineInternal(coroutine, segment, layer, true, tag, new CoroutineHandle(this._instanceID), true);
		}

		private CoroutineHandle RunCoroutineInternal(IEnumerator<float> coroutine, Segment segment, int layer, bool layerHasValue, string tag, CoroutineHandle handle, bool prewarm)
		{
			Timing.ProcessIndex processIndex = new Timing.ProcessIndex
			{
				seg = segment
			};
			if (this._handleToIndex.ContainsKey(handle))
			{
				this._indexToHandle.Remove(this._handleToIndex[handle]);
				this._handleToIndex.Remove(handle);
			}
			float num = this.localTime;
			float num2 = this.deltaTime;
			CoroutineHandle currentCoroutine = this.currentCoroutine;
			this.currentCoroutine = handle;
			try
			{
				switch (segment)
				{
					case Segment.Update:
						{
							if (this._nextUpdateProcessSlot >= this.UpdateProcesses.Length)
							{
								IEnumerator<float>[] updateProcesses = this.UpdateProcesses;
								bool[] updatePaused = this.UpdatePaused;
								bool[] updateHeld = this.UpdateHeld;
								ushort num3 = (ushort)this.UpdateProcesses.Length;
								ushort num4 = 64;
								ushort expansions = this._expansions;
								this._expansions = (ushort)(expansions + 1);
								this.UpdateProcesses = new IEnumerator<float>[(int)(num3 + num4 * expansions)];
								this.UpdatePaused = new bool[this.UpdateProcesses.Length];
								this.UpdateHeld = new bool[this.UpdateProcesses.Length];
								for (int i = 0; i < updateProcesses.Length; i++)
								{
									this.UpdateProcesses[i] = updateProcesses[i];
									this.UpdatePaused[i] = updatePaused[i];
									this.UpdateHeld[i] = updateHeld[i];
								}
							}
							if (this.UpdateTimeValues(processIndex.seg))
							{
								this._lastUpdateProcessSlot = this._nextUpdateProcessSlot;
							}
							int num5 = this._nextUpdateProcessSlot;
							this._nextUpdateProcessSlot = num5 + 1;
							processIndex.i = num5;
							this.UpdateProcesses[processIndex.i] = coroutine;
							if (tag != null)
							{
								this.AddTagOnInstance(tag, handle);
							}
							if (layerHasValue)
							{
								this.AddLayerOnInstance(layer, handle);
							}
							this._indexToHandle.Add(processIndex, handle);
							this._handleToIndex.Add(handle, processIndex);
							while (prewarm)
							{
								if (!this.UpdateProcesses[processIndex.i].MoveNext())
								{
									if (this._indexToHandle.ContainsKey(processIndex))
									{
										this.KillCoroutinesOnInstance(this._indexToHandle[processIndex]);
									}
									prewarm = false;
								}
								else if (this.UpdateProcesses[processIndex.i] != null && float.IsNaN(this.UpdateProcesses[processIndex.i].Current))
								{
									if (Timing.ReplacementFunction != null)
									{
										this.UpdateProcesses[processIndex.i] = Timing.ReplacementFunction(this.UpdateProcesses[processIndex.i], this._indexToHandle[processIndex]);
										Timing.ReplacementFunction = null;
									}
									prewarm = (!this.UpdatePaused[processIndex.i] && !this.UpdateHeld[processIndex.i]);
								}
								else
								{
									prewarm = false;
								}
							}
							goto IL_D75;
						}
					case Segment.FixedUpdate:
						{
							if (this._nextFixedUpdateProcessSlot >= this.FixedUpdateProcesses.Length)
							{
								IEnumerator<float>[] fixedUpdateProcesses = this.FixedUpdateProcesses;
								bool[] fixedUpdatePaused = this.FixedUpdatePaused;
								bool[] fixedUpdateHeld = this.FixedUpdateHeld;
								ushort num6 = (ushort)this.FixedUpdateProcesses.Length;
								ushort num7 = 64;
								ushort expansions = this._expansions;
								this._expansions = (ushort)(expansions + 1);
								this.FixedUpdateProcesses = new IEnumerator<float>[(int)(num6 + num7 * expansions)];
								this.FixedUpdatePaused = new bool[this.FixedUpdateProcesses.Length];
								this.FixedUpdateHeld = new bool[this.FixedUpdateProcesses.Length];
								for (int j = 0; j < fixedUpdateProcesses.Length; j++)
								{
									this.FixedUpdateProcesses[j] = fixedUpdateProcesses[j];
									this.FixedUpdatePaused[j] = fixedUpdatePaused[j];
									this.FixedUpdateHeld[j] = fixedUpdateHeld[j];
								}
							}
							if (this.UpdateTimeValues(processIndex.seg))
							{
								this._lastFixedUpdateProcessSlot = this._nextFixedUpdateProcessSlot;
							}
							int num5 = this._nextFixedUpdateProcessSlot;
							this._nextFixedUpdateProcessSlot = num5 + 1;
							processIndex.i = num5;
							this.FixedUpdateProcesses[processIndex.i] = coroutine;
							if (tag != null)
							{
								this.AddTagOnInstance(tag, handle);
							}
							if (layerHasValue)
							{
								this.AddLayerOnInstance(layer, handle);
							}
							this._indexToHandle.Add(processIndex, handle);
							this._handleToIndex.Add(handle, processIndex);
							while (prewarm)
							{
								if (!this.FixedUpdateProcesses[processIndex.i].MoveNext())
								{
									if (this._indexToHandle.ContainsKey(processIndex))
									{
										this.KillCoroutinesOnInstance(this._indexToHandle[processIndex]);
									}
									prewarm = false;
								}
								else if (this.FixedUpdateProcesses[processIndex.i] != null && float.IsNaN(this.FixedUpdateProcesses[processIndex.i].Current))
								{
									if (Timing.ReplacementFunction != null)
									{
										this.FixedUpdateProcesses[processIndex.i] = Timing.ReplacementFunction(this.FixedUpdateProcesses[processIndex.i], this._indexToHandle[processIndex]);
										Timing.ReplacementFunction = null;
									}
									prewarm = (!this.FixedUpdatePaused[processIndex.i] && !this.FixedUpdateHeld[processIndex.i]);
								}
								else
								{
									prewarm = false;
								}
							}
							goto IL_D75;
						}
					case Segment.LateUpdate:
						{
							if (this._nextLateUpdateProcessSlot >= this.LateUpdateProcesses.Length)
							{
								IEnumerator<float>[] lateUpdateProcesses = this.LateUpdateProcesses;
								bool[] lateUpdatePaused = this.LateUpdatePaused;
								bool[] lateUpdateHeld = this.LateUpdateHeld;
								ushort num8 = (ushort)this.LateUpdateProcesses.Length;
								ushort num9 = 64;
								ushort expansions = this._expansions;
								this._expansions = (ushort)(expansions + 1);
								this.LateUpdateProcesses = new IEnumerator<float>[(int)(num8 + num9 * expansions)];
								this.LateUpdatePaused = new bool[this.LateUpdateProcesses.Length];
								this.LateUpdateHeld = new bool[this.LateUpdateProcesses.Length];
								for (int k = 0; k < lateUpdateProcesses.Length; k++)
								{
									this.LateUpdateProcesses[k] = lateUpdateProcesses[k];
									this.LateUpdatePaused[k] = lateUpdatePaused[k];
									this.LateUpdateHeld[k] = lateUpdateHeld[k];
								}
							}
							if (this.UpdateTimeValues(processIndex.seg))
							{
								this._lastLateUpdateProcessSlot = this._nextLateUpdateProcessSlot;
							}
							int num5 = this._nextLateUpdateProcessSlot;
							this._nextLateUpdateProcessSlot = num5 + 1;
							processIndex.i = num5;
							this.LateUpdateProcesses[processIndex.i] = coroutine;
							if (tag != null)
							{
								this.AddTagOnInstance(tag, handle);
							}
							if (layerHasValue)
							{
								this.AddLayerOnInstance(layer, handle);
							}
							this._indexToHandle.Add(processIndex, handle);
							this._handleToIndex.Add(handle, processIndex);
							while (prewarm)
							{
								if (!this.LateUpdateProcesses[processIndex.i].MoveNext())
								{
									if (this._indexToHandle.ContainsKey(processIndex))
									{
										this.KillCoroutinesOnInstance(this._indexToHandle[processIndex]);
									}
									prewarm = false;
								}
								else if (this.LateUpdateProcesses[processIndex.i] != null && float.IsNaN(this.LateUpdateProcesses[processIndex.i].Current))
								{
									if (Timing.ReplacementFunction != null)
									{
										this.LateUpdateProcesses[processIndex.i] = Timing.ReplacementFunction(this.LateUpdateProcesses[processIndex.i], this._indexToHandle[processIndex]);
										Timing.ReplacementFunction = null;
									}
									prewarm = (!this.LateUpdatePaused[processIndex.i] && !this.LateUpdateHeld[processIndex.i]);
								}
								else
								{
									prewarm = false;
								}
							}
							goto IL_D75;
						}
					case Segment.SlowUpdate:
						{
							if (this._nextSlowUpdateProcessSlot >= this.SlowUpdateProcesses.Length)
							{
								IEnumerator<float>[] slowUpdateProcesses = this.SlowUpdateProcesses;
								bool[] slowUpdatePaused = this.SlowUpdatePaused;
								bool[] slowUpdateHeld = this.SlowUpdateHeld;
								ushort num10 = (ushort)this.SlowUpdateProcesses.Length;
								ushort num11 = 64;
								ushort expansions = this._expansions;
								this._expansions = (ushort)(expansions + 1);
								this.SlowUpdateProcesses = new IEnumerator<float>[(int)(num10 + num11 * expansions)];
								this.SlowUpdatePaused = new bool[this.SlowUpdateProcesses.Length];
								this.SlowUpdateHeld = new bool[this.SlowUpdateProcesses.Length];
								for (int l = 0; l < slowUpdateProcesses.Length; l++)
								{
									this.SlowUpdateProcesses[l] = slowUpdateProcesses[l];
									this.SlowUpdatePaused[l] = slowUpdatePaused[l];
									this.SlowUpdateHeld[l] = slowUpdateHeld[l];
								}
							}
							if (this.UpdateTimeValues(processIndex.seg))
							{
								this._lastSlowUpdateProcessSlot = this._nextSlowUpdateProcessSlot;
							}
							int num5 = this._nextSlowUpdateProcessSlot;
							this._nextSlowUpdateProcessSlot = num5 + 1;
							processIndex.i = num5;
							this.SlowUpdateProcesses[processIndex.i] = coroutine;
							if (tag != null)
							{
								this.AddTagOnInstance(tag, handle);
							}
							if (layerHasValue)
							{
								this.AddLayerOnInstance(layer, handle);
							}
							this._indexToHandle.Add(processIndex, handle);
							this._handleToIndex.Add(handle, processIndex);
							while (prewarm)
							{
								if (!this.SlowUpdateProcesses[processIndex.i].MoveNext())
								{
									if (this._indexToHandle.ContainsKey(processIndex))
									{
										this.KillCoroutinesOnInstance(this._indexToHandle[processIndex]);
									}
									prewarm = false;
								}
								else if (this.SlowUpdateProcesses[processIndex.i] != null && float.IsNaN(this.SlowUpdateProcesses[processIndex.i].Current))
								{
									if (Timing.ReplacementFunction != null)
									{
										this.SlowUpdateProcesses[processIndex.i] = Timing.ReplacementFunction(this.SlowUpdateProcesses[processIndex.i], this._indexToHandle[processIndex]);
										Timing.ReplacementFunction = null;
									}
									prewarm = (!this.SlowUpdatePaused[processIndex.i] && !this.SlowUpdateHeld[processIndex.i]);
								}
								else
								{
									prewarm = false;
								}
							}
							goto IL_D75;
						}
					case Segment.RealtimeUpdate:
						{
							if (this._nextRealtimeUpdateProcessSlot >= this.RealtimeUpdateProcesses.Length)
							{
								IEnumerator<float>[] realtimeUpdateProcesses = this.RealtimeUpdateProcesses;
								bool[] realtimeUpdatePaused = this.RealtimeUpdatePaused;
								bool[] realtimeUpdateHeld = this.RealtimeUpdateHeld;
								ushort num12 = (ushort)this.RealtimeUpdateProcesses.Length;
								ushort num13 = 64;
								ushort expansions = this._expansions;
								this._expansions = (ushort)(expansions + 1);
								this.RealtimeUpdateProcesses = new IEnumerator<float>[(int)(num12 + num13 * expansions)];
								this.RealtimeUpdatePaused = new bool[this.RealtimeUpdateProcesses.Length];
								this.RealtimeUpdateHeld = new bool[this.RealtimeUpdateProcesses.Length];
								for (int m = 0; m < realtimeUpdateProcesses.Length; m++)
								{
									this.RealtimeUpdateProcesses[m] = realtimeUpdateProcesses[m];
									this.RealtimeUpdatePaused[m] = realtimeUpdatePaused[m];
									this.RealtimeUpdateHeld[m] = realtimeUpdateHeld[m];
								}
							}
							if (this.UpdateTimeValues(processIndex.seg))
							{
								this._lastRealtimeUpdateProcessSlot = this._nextRealtimeUpdateProcessSlot;
							}
							int num5 = this._nextRealtimeUpdateProcessSlot;
							this._nextRealtimeUpdateProcessSlot = num5 + 1;
							processIndex.i = num5;
							this.RealtimeUpdateProcesses[processIndex.i] = coroutine;
							if (tag != null)
							{
								this.AddTagOnInstance(tag, handle);
							}
							if (layerHasValue)
							{
								this.AddLayerOnInstance(layer, handle);
							}
							this._indexToHandle.Add(processIndex, handle);
							this._handleToIndex.Add(handle, processIndex);
							while (prewarm)
							{
								if (!this.RealtimeUpdateProcesses[processIndex.i].MoveNext())
								{
									if (this._indexToHandle.ContainsKey(processIndex))
									{
										this.KillCoroutinesOnInstance(this._indexToHandle[processIndex]);
									}
									prewarm = false;
								}
								else if (this.RealtimeUpdateProcesses[processIndex.i] != null && float.IsNaN(this.RealtimeUpdateProcesses[processIndex.i].Current))
								{
									if (Timing.ReplacementFunction != null)
									{
										this.RealtimeUpdateProcesses[processIndex.i] = Timing.ReplacementFunction(this.RealtimeUpdateProcesses[processIndex.i], this._indexToHandle[processIndex]);
										Timing.ReplacementFunction = null;
									}
									prewarm = (!this.RealtimeUpdatePaused[processIndex.i] && !this.RealtimeUpdateHeld[processIndex.i]);
								}
								else
								{
									prewarm = false;
								}
							}
							goto IL_D75;
						}
					case Segment.EndOfFrame:
						{
							if (this._nextEndOfFrameProcessSlot >= this.EndOfFrameProcesses.Length)
							{
								IEnumerator<float>[] endOfFrameProcesses = this.EndOfFrameProcesses;
								bool[] endOfFramePaused = this.EndOfFramePaused;
								bool[] endOfFrameHeld = this.EndOfFrameHeld;
								ushort num14 = (ushort)this.EndOfFrameProcesses.Length;
								ushort num15 = 64;
								ushort expansions = this._expansions;
								this._expansions = (ushort)(expansions + 1);
								this.EndOfFrameProcesses = new IEnumerator<float>[(int)(num14 + num15 * expansions)];
								this.EndOfFramePaused = new bool[this.EndOfFrameProcesses.Length];
								this.EndOfFrameHeld = new bool[this.EndOfFrameProcesses.Length];
								for (int n = 0; n < endOfFrameProcesses.Length; n++)
								{
									this.EndOfFrameProcesses[n] = endOfFrameProcesses[n];
									this.EndOfFramePaused[n] = endOfFramePaused[n];
									this.EndOfFrameHeld[n] = endOfFrameHeld[n];
								}
							}
							if (this.UpdateTimeValues(processIndex.seg))
							{
								this._lastEndOfFrameProcessSlot = this._nextEndOfFrameProcessSlot;
							}
							int num5 = this._nextEndOfFrameProcessSlot;
							this._nextEndOfFrameProcessSlot = num5 + 1;
							processIndex.i = num5;
							this.EndOfFrameProcesses[processIndex.i] = coroutine;
							if (tag != null)
							{
								this.AddTagOnInstance(tag, handle);
							}
							if (layerHasValue)
							{
								this.AddLayerOnInstance(layer, handle);
							}
							this._indexToHandle.Add(processIndex, handle);
							this._handleToIndex.Add(handle, processIndex);
							this._eofWatcherHandle = this.RunCoroutineSingletonOnInstance(this._EOFPumpWatcher(), this._eofWatcherHandle, SingletonBehavior.Abort);
							goto IL_D75;
						}
					case Segment.ManualTimeframe:
						{
							if (this._nextManualTimeframeProcessSlot >= this.ManualTimeframeProcesses.Length)
							{
								IEnumerator<float>[] manualTimeframeProcesses = this.ManualTimeframeProcesses;
								bool[] manualTimeframePaused = this.ManualTimeframePaused;
								bool[] manualTimeframeHeld = this.ManualTimeframeHeld;
								ushort num16 = (ushort)this.ManualTimeframeProcesses.Length;
								ushort num17 = 64;
								ushort expansions = this._expansions;
								this._expansions = (ushort)(expansions + 1);
								this.ManualTimeframeProcesses = new IEnumerator<float>[(int)(num16 + num17 * expansions)];
								this.ManualTimeframePaused = new bool[this.ManualTimeframeProcesses.Length];
								this.ManualTimeframeHeld = new bool[this.ManualTimeframeProcesses.Length];
								for (int num18 = 0; num18 < manualTimeframeProcesses.Length; num18++)
								{
									this.ManualTimeframeProcesses[num18] = manualTimeframeProcesses[num18];
									this.ManualTimeframePaused[num18] = manualTimeframePaused[num18];
									this.ManualTimeframeHeld[num18] = manualTimeframeHeld[num18];
								}
							}
							if (this.UpdateTimeValues(processIndex.seg))
							{
								this._lastManualTimeframeProcessSlot = this._nextManualTimeframeProcessSlot;
							}
							int num5 = this._nextManualTimeframeProcessSlot;
							this._nextManualTimeframeProcessSlot = num5 + 1;
							processIndex.i = num5;
							this.ManualTimeframeProcesses[processIndex.i] = coroutine;
							if (tag != null)
							{
								this.AddTagOnInstance(tag, handle);
							}
							if (layerHasValue)
							{
								this.AddLayerOnInstance(layer, handle);
							}
							this._indexToHandle.Add(processIndex, handle);
							this._handleToIndex.Add(handle, processIndex);
							goto IL_D75;
						}
				}
				handle = default(CoroutineHandle);
			IL_D75:;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			this.localTime = num;
			this.deltaTime = num2;
			this.currentCoroutine = currentCoroutine;
			return handle;
		}

		public static int KillCoroutines()
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.KillCoroutinesOnInstance();
			}
			return 0;
		}

		public int KillCoroutinesOnInstance()
		{
			int result = this._nextUpdateProcessSlot + this._nextLateUpdateProcessSlot + this._nextFixedUpdateProcessSlot + this._nextSlowUpdateProcessSlot + this._nextRealtimeUpdateProcessSlot + this._nextEditorUpdateProcessSlot + this._nextEditorSlowUpdateProcessSlot + this._nextEndOfFrameProcessSlot + this._nextManualTimeframeProcessSlot;
			this.UpdateProcesses = new IEnumerator<float>[256];
			this.UpdatePaused = new bool[256];
			this.UpdateHeld = new bool[256];
			this.UpdateCoroutines = 0;
			this._nextUpdateProcessSlot = 0;
			this.LateUpdateProcesses = new IEnumerator<float>[8];
			this.LateUpdatePaused = new bool[8];
			this.LateUpdateHeld = new bool[8];
			this.LateUpdateCoroutines = 0;
			this._nextLateUpdateProcessSlot = 0;
			this.FixedUpdateProcesses = new IEnumerator<float>[64];
			this.FixedUpdatePaused = new bool[64];
			this.FixedUpdateHeld = new bool[64];
			this.FixedUpdateCoroutines = 0;
			this._nextFixedUpdateProcessSlot = 0;
			this.SlowUpdateProcesses = new IEnumerator<float>[64];
			this.SlowUpdatePaused = new bool[64];
			this.SlowUpdateHeld = new bool[64];
			this.SlowUpdateCoroutines = 0;
			this._nextSlowUpdateProcessSlot = 0;
			this.RealtimeUpdateProcesses = new IEnumerator<float>[8];
			this.RealtimeUpdatePaused = new bool[8];
			this.RealtimeUpdateHeld = new bool[8];
			this.RealtimeUpdateCoroutines = 0;
			this._nextRealtimeUpdateProcessSlot = 0;
			this.EditorUpdateProcesses = new IEnumerator<float>[8];
			this.EditorUpdatePaused = new bool[8];
			this.EditorUpdateHeld = new bool[8];
			this.EditorUpdateCoroutines = 0;
			this._nextEditorUpdateProcessSlot = 0;
			this.EditorSlowUpdateProcesses = new IEnumerator<float>[8];
			this.EditorSlowUpdatePaused = new bool[8];
			this.EditorSlowUpdateHeld = new bool[8];
			this.EditorSlowUpdateCoroutines = 0;
			this._nextEditorSlowUpdateProcessSlot = 0;
			this.EndOfFrameProcesses = new IEnumerator<float>[8];
			this.EndOfFramePaused = new bool[8];
			this.EndOfFrameHeld = new bool[8];
			this.EndOfFrameCoroutines = 0;
			this._nextEndOfFrameProcessSlot = 0;
			this.ManualTimeframeProcesses = new IEnumerator<float>[8];
			this.ManualTimeframePaused = new bool[8];
			this.ManualTimeframeHeld = new bool[8];
			this.ManualTimeframeCoroutines = 0;
			this._nextManualTimeframeProcessSlot = 0;
			this._processTags.Clear();
			this._taggedProcesses.Clear();
			this._processLayers.Clear();
			this._layeredProcesses.Clear();
			this._handleToIndex.Clear();
			this._indexToHandle.Clear();
			this._waitingTriggers.Clear();
			this._expansions = (ushort)(this._expansions / 2 + 1);
			Timing.Links.Clear();
			return result;
		}

		public static int KillCoroutines(params CoroutineHandle[] handles)
		{
			int num = 0;
			for (int i = 0; i < handles.Length; i++)
			{
				num += ((Timing.ActiveInstances[(int)handles[i].Key] != null) ? Timing.GetInstance(handles[i].Key).KillCoroutinesOnInstance(handles[i]) : 0);
			}
			return num;
		}

		public int KillCoroutinesOnInstance(CoroutineHandle handle)
		{
			int num = 0;
			if (this._handleToIndex.ContainsKey(handle))
			{
				if (this._waitingTriggers.ContainsKey(handle))
				{
					this.CloseWaitingProcess(handle);
				}
				if (this.Nullify(handle))
				{
					num++;
				}
				this.RemoveGraffiti(handle);
			}
			if (Timing.Links.ContainsKey(handle))
			{
				HashSet<CoroutineHandle>.Enumerator enumerator = Timing.Links[handle].GetEnumerator();
				Timing.Links.Remove(handle);
				while (enumerator.MoveNext())
				{
					CoroutineHandle coroutineHandle = enumerator.Current;
					num += Timing.KillCoroutines(new CoroutineHandle[]
					{
						coroutineHandle
					});
				}
			}
			return num;
		}

		public static int KillCoroutines(GameObject gameObj)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.KillCoroutinesOnInstance(gameObj.GetInstanceID());
			}
			return 0;
		}

		public int KillCoroutinesOnInstance(GameObject gameObj)
		{
			return this.KillCoroutinesOnInstance(gameObj.GetInstanceID());
		}

		public static int KillCoroutines(int layer)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.KillCoroutinesOnInstance(layer);
			}
			return 0;
		}

		public int KillCoroutinesOnInstance(int layer)
		{
			int num = 0;
			while (this._layeredProcesses.ContainsKey(layer))
			{
				HashSet<CoroutineHandle>.Enumerator enumerator = this._layeredProcesses[layer].GetEnumerator();
				enumerator.MoveNext();
				if (this.Nullify(enumerator.Current))
				{
					if (this._waitingTriggers.ContainsKey(enumerator.Current))
					{
						this.CloseWaitingProcess(enumerator.Current);
					}
					num++;
				}
				this.RemoveGraffiti(enumerator.Current);
				if (Timing.Links.ContainsKey(enumerator.Current))
				{
					HashSet<CoroutineHandle>.Enumerator enumerator2 = Timing.Links[enumerator.Current].GetEnumerator();
					Timing.Links.Remove(enumerator.Current);
					while (enumerator2.MoveNext())
					{
						CoroutineHandle coroutineHandle = enumerator2.Current;
						num += Timing.KillCoroutines(new CoroutineHandle[]
						{
							coroutineHandle
						});
					}
				}
			}
			return num;
		}

		public static int KillCoroutines(string tag)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.KillCoroutinesOnInstance(tag);
			}
			return 0;
		}

		public int KillCoroutinesOnInstance(string tag)
		{
			if (tag == null)
			{
				return 0;
			}
			int num = 0;
			while (this._taggedProcesses.ContainsKey(tag))
			{
				HashSet<CoroutineHandle>.Enumerator enumerator = this._taggedProcesses[tag].GetEnumerator();
				enumerator.MoveNext();
				if (this.Nullify(this._handleToIndex[enumerator.Current]))
				{
					if (this._waitingTriggers.ContainsKey(enumerator.Current))
					{
						this.CloseWaitingProcess(enumerator.Current);
					}
					num++;
				}
				this.RemoveGraffiti(enumerator.Current);
				if (Timing.Links.ContainsKey(enumerator.Current))
				{
					HashSet<CoroutineHandle>.Enumerator enumerator2 = Timing.Links[enumerator.Current].GetEnumerator();
					Timing.Links.Remove(enumerator.Current);
					while (enumerator2.MoveNext())
					{
						CoroutineHandle coroutineHandle = enumerator2.Current;
						num += Timing.KillCoroutines(new CoroutineHandle[]
						{
							coroutineHandle
						});
					}
				}
			}
			return num;
		}

		public static int KillCoroutines(GameObject gameObj, string tag)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.KillCoroutinesOnInstance(gameObj.GetInstanceID(), tag);
			}
			return 0;
		}

		public int KillCoroutinesOnInstance(GameObject gameObj, string tag)
		{
			return this.KillCoroutinesOnInstance(gameObj.GetInstanceID(), tag);
		}

		public static int KillCoroutines(int layer, string tag)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.KillCoroutinesOnInstance(layer, tag);
			}
			return 0;
		}

		public int KillCoroutinesOnInstance(int layer, string tag)
		{
			if (tag == null)
			{
				return this.KillCoroutinesOnInstance(layer);
			}
			if (!this._layeredProcesses.ContainsKey(layer) || !this._taggedProcesses.ContainsKey(tag))
			{
				return 0;
			}
			int num = 0;
			HashSet<CoroutineHandle>.Enumerator enumerator = this._taggedProcesses[tag].GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!this.CoindexIsNull(this._handleToIndex[enumerator.Current]) && this._layeredProcesses[layer].Contains(enumerator.Current) && this.Nullify(enumerator.Current))
				{
					if (this._waitingTriggers.ContainsKey(enumerator.Current))
					{
						this.CloseWaitingProcess(enumerator.Current);
					}
					num++;
					this.RemoveGraffiti(enumerator.Current);
					if (Timing.Links.ContainsKey(enumerator.Current))
					{
						HashSet<CoroutineHandle>.Enumerator enumerator2 = Timing.Links[enumerator.Current].GetEnumerator();
						Timing.Links.Remove(enumerator.Current);
						while (enumerator2.MoveNext())
						{
							CoroutineHandle coroutineHandle = enumerator2.Current;
							Timing.KillCoroutines(new CoroutineHandle[]
							{
								coroutineHandle
							});
						}
					}
					if (!this._taggedProcesses.ContainsKey(tag) || !this._layeredProcesses.ContainsKey(layer))
					{
						break;
					}
					enumerator = this._taggedProcesses[tag].GetEnumerator();
				}
			}
			return num;
		}

		public static Timing GetInstance(byte ID)
		{
			if (ID >= 16)
			{
				return null;
			}
			return Timing.ActiveInstances[(int)ID];
		}

		public static float WaitForSeconds(float waitTime)
		{
			if (float.IsNaN(waitTime))
			{
				waitTime = 0f;
			}
			return Timing.LocalTime + waitTime;
		}

		public float WaitForSecondsOnInstance(float waitTime)
		{
			if (float.IsNaN(waitTime))
			{
				waitTime = 0f;
			}
			return this.localTime + waitTime;
		}

		private bool UpdateTimeValues(Segment segment)
		{
			switch (segment)
			{
				case Segment.Update:
					if (this._currentUpdateFrame != Time.frameCount)
					{
						this.deltaTime = Time.deltaTime;
						this._lastUpdateTime += this.deltaTime;
						this.localTime = this._lastUpdateTime;
						this._currentUpdateFrame = Time.frameCount;
						return true;
					}
					this.deltaTime = Time.deltaTime;
					this.localTime = this._lastUpdateTime;
					return false;
				case Segment.FixedUpdate:
					this.deltaTime = Time.fixedDeltaTime;
					this.localTime = Time.fixedTime;
					if (this._lastFixedUpdateTime + 0.0001f < Time.fixedTime)
					{
						this._lastFixedUpdateTime = Time.fixedTime;
						return true;
					}
					return false;
				case Segment.LateUpdate:
					if (this._currentLateUpdateFrame != Time.frameCount)
					{
						this.deltaTime = Time.deltaTime;
						this._lastLateUpdateTime += this.deltaTime;
						this.localTime = this._lastLateUpdateTime;
						this._currentLateUpdateFrame = Time.frameCount;
						return true;
					}
					this.deltaTime = Time.deltaTime;
					this.localTime = this._lastLateUpdateTime;
					return false;
				case Segment.SlowUpdate:
					if (this._currentSlowUpdateFrame != Time.frameCount)
					{
						this.deltaTime = (this._lastSlowUpdateDeltaTime = Time.realtimeSinceStartup - this._lastSlowUpdateTime);
						this.localTime = (this._lastSlowUpdateTime = Time.realtimeSinceStartup);
						this._currentSlowUpdateFrame = Time.frameCount;
						return true;
					}
					this.localTime = this._lastSlowUpdateTime;
					this.deltaTime = this._lastSlowUpdateDeltaTime;
					return false;
				case Segment.RealtimeUpdate:
					if (this._currentRealtimeUpdateFrame != Time.frameCount)
					{
						this.deltaTime = Time.unscaledDeltaTime;
						this._lastRealtimeUpdateTime += this.deltaTime;
						this.localTime = this._lastRealtimeUpdateTime;
						this._currentRealtimeUpdateFrame = Time.frameCount;
						return true;
					}
					this.deltaTime = Time.unscaledDeltaTime;
					this.localTime = this._lastRealtimeUpdateTime;
					return false;
				case Segment.EndOfFrame:
					if (this._currentEndOfFrameFrame != Time.frameCount)
					{
						this.deltaTime = Time.deltaTime;
						this._lastEndOfFrameTime += this.deltaTime;
						this.localTime = this._lastEndOfFrameTime;
						this._currentEndOfFrameFrame = Time.frameCount;
						return true;
					}
					this.deltaTime = Time.deltaTime;
					this.localTime = this._lastEndOfFrameTime;
					return false;
				case Segment.ManualTimeframe:
					{
						float num = (this.SetManualTimeframeTime == null) ? Time.time : this.SetManualTimeframeTime(this._lastManualTimeframeTime);
						if ((double)this._lastManualTimeframeTime + 0.0001 < (double)num && (double)this._lastManualTimeframeTime - 0.0001 > (double)num)
						{
							this.localTime = num;
							this.deltaTime = this.localTime - this._lastManualTimeframeTime;
							if (this.deltaTime > Time.maximumDeltaTime)
							{
								this.deltaTime = Time.maximumDeltaTime;
							}
							this._lastManualTimeframeDeltaTime = this.deltaTime;
							this._lastManualTimeframeTime = num;
							return true;
						}
						this.deltaTime = this._lastManualTimeframeDeltaTime;
						this.localTime = this._lastManualTimeframeTime;
						return false;
					}
			}
			return true;
		}

		private float GetSegmentTime(Segment segment)
		{
			switch (segment)
			{
				case Segment.Update:
					if (this._currentUpdateFrame == Time.frameCount)
					{
						return this._lastUpdateTime;
					}
					return this._lastUpdateTime + Time.deltaTime;
				case Segment.FixedUpdate:
					return Time.fixedTime;
				case Segment.LateUpdate:
					if (this._currentUpdateFrame == Time.frameCount)
					{
						return this._lastLateUpdateTime;
					}
					return this._lastLateUpdateTime + Time.deltaTime;
				case Segment.SlowUpdate:
					return Time.realtimeSinceStartup;
				case Segment.RealtimeUpdate:
					if (this._currentRealtimeUpdateFrame == Time.frameCount)
					{
						return this._lastRealtimeUpdateTime;
					}
					return this._lastRealtimeUpdateTime + Time.unscaledDeltaTime;
				case Segment.EndOfFrame:
					if (this._currentUpdateFrame == Time.frameCount)
					{
						return this._lastEndOfFrameTime;
					}
					return this._lastEndOfFrameTime + Time.deltaTime;
				case Segment.ManualTimeframe:
					return this._lastManualTimeframeTime;
			}
			return 0f;
		}

		public static int PauseCoroutines()
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.PauseCoroutinesOnInstance();
			}
			return 0;
		}

		public int PauseCoroutinesOnInstance()
		{
			int num = 0;
			for (int i = 0; i < this._nextUpdateProcessSlot; i++)
			{
				if (!this.UpdatePaused[i] && this.UpdateProcesses[i] != null)
				{
					num++;
					this.UpdatePaused[i] = true;
					if (this.UpdateProcesses[i].Current > this.GetSegmentTime(Segment.Update))
					{
						this.UpdateProcesses[i] = this._InjectDelay(this.UpdateProcesses[i], this.UpdateProcesses[i].Current - this.GetSegmentTime(Segment.Update));
					}
				}
			}
			for (int i = 0; i < this._nextLateUpdateProcessSlot; i++)
			{
				if (!this.LateUpdatePaused[i] && this.LateUpdateProcesses[i] != null)
				{
					num++;
					this.LateUpdatePaused[i] = true;
					if (this.LateUpdateProcesses[i].Current > this.GetSegmentTime(Segment.LateUpdate))
					{
						this.LateUpdateProcesses[i] = this._InjectDelay(this.LateUpdateProcesses[i], this.LateUpdateProcesses[i].Current - this.GetSegmentTime(Segment.LateUpdate));
					}
				}
			}
			for (int i = 0; i < this._nextFixedUpdateProcessSlot; i++)
			{
				if (!this.FixedUpdatePaused[i] && this.FixedUpdateProcesses[i] != null)
				{
					num++;
					this.FixedUpdatePaused[i] = true;
					if (this.FixedUpdateProcesses[i].Current > this.GetSegmentTime(Segment.FixedUpdate))
					{
						this.FixedUpdateProcesses[i] = this._InjectDelay(this.FixedUpdateProcesses[i], this.FixedUpdateProcesses[i].Current - this.GetSegmentTime(Segment.FixedUpdate));
					}
				}
			}
			for (int i = 0; i < this._nextSlowUpdateProcessSlot; i++)
			{
				if (!this.SlowUpdatePaused[i] && this.SlowUpdateProcesses[i] != null)
				{
					num++;
					this.SlowUpdatePaused[i] = true;
					if (this.SlowUpdateProcesses[i].Current > this.GetSegmentTime(Segment.SlowUpdate))
					{
						this.SlowUpdateProcesses[i] = this._InjectDelay(this.SlowUpdateProcesses[i], this.SlowUpdateProcesses[i].Current - this.GetSegmentTime(Segment.SlowUpdate));
					}
				}
			}
			for (int i = 0; i < this._nextRealtimeUpdateProcessSlot; i++)
			{
				if (!this.RealtimeUpdatePaused[i] && this.RealtimeUpdateProcesses[i] != null)
				{
					num++;
					this.RealtimeUpdatePaused[i] = true;
					if (this.RealtimeUpdateProcesses[i].Current > this.GetSegmentTime(Segment.RealtimeUpdate))
					{
						this.RealtimeUpdateProcesses[i] = this._InjectDelay(this.RealtimeUpdateProcesses[i], this.RealtimeUpdateProcesses[i].Current - this.GetSegmentTime(Segment.RealtimeUpdate));
					}
				}
			}
			for (int i = 0; i < this._nextEditorUpdateProcessSlot; i++)
			{
				if (!this.EditorUpdatePaused[i] && this.EditorUpdateProcesses[i] != null)
				{
					num++;
					this.EditorUpdatePaused[i] = true;
					if (this.EditorUpdateProcesses[i].Current > this.GetSegmentTime(Segment.EditorUpdate))
					{
						this.EditorUpdateProcesses[i] = this._InjectDelay(this.EditorUpdateProcesses[i], this.EditorUpdateProcesses[i].Current - this.GetSegmentTime(Segment.EditorUpdate));
					}
				}
			}
			for (int i = 0; i < this._nextEditorSlowUpdateProcessSlot; i++)
			{
				if (!this.EditorSlowUpdatePaused[i] && this.EditorSlowUpdateProcesses[i] != null)
				{
					num++;
					this.EditorSlowUpdatePaused[i] = true;
					if (this.EditorSlowUpdateProcesses[i].Current > this.GetSegmentTime(Segment.EditorSlowUpdate))
					{
						this.EditorSlowUpdateProcesses[i] = this._InjectDelay(this.EditorSlowUpdateProcesses[i], this.EditorSlowUpdateProcesses[i].Current - this.GetSegmentTime(Segment.EditorSlowUpdate));
					}
				}
			}
			for (int i = 0; i < this._nextEndOfFrameProcessSlot; i++)
			{
				if (!this.EndOfFramePaused[i] && this.EndOfFrameProcesses[i] != null)
				{
					num++;
					this.EndOfFramePaused[i] = true;
					if (this.EndOfFrameProcesses[i].Current > this.GetSegmentTime(Segment.EndOfFrame))
					{
						this.EndOfFrameProcesses[i] = this._InjectDelay(this.EndOfFrameProcesses[i], this.EndOfFrameProcesses[i].Current - this.GetSegmentTime(Segment.EndOfFrame));
					}
				}
			}
			for (int i = 0; i < this._nextManualTimeframeProcessSlot; i++)
			{
				if (!this.ManualTimeframePaused[i] && this.ManualTimeframeProcesses[i] != null)
				{
					num++;
					this.ManualTimeframePaused[i] = true;
					if (this.ManualTimeframeProcesses[i].Current > this.GetSegmentTime(Segment.ManualTimeframe))
					{
						this.ManualTimeframeProcesses[i] = this._InjectDelay(this.ManualTimeframeProcesses[i], this.ManualTimeframeProcesses[i].Current - this.GetSegmentTime(Segment.ManualTimeframe));
					}
				}
			}
			Dictionary<CoroutineHandle, HashSet<CoroutineHandle>>.Enumerator enumerator = Timing.Links.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Dictionary<CoroutineHandle, Timing.ProcessIndex> handleToIndex = this._handleToIndex;
				KeyValuePair<CoroutineHandle, HashSet<CoroutineHandle>> keyValuePair = enumerator.Current;
				if (handleToIndex.ContainsKey(keyValuePair.Key))
				{
					keyValuePair = enumerator.Current;
					foreach (CoroutineHandle coroutineHandle in keyValuePair.Value)
					{
						num += Timing.PauseCoroutines(new CoroutineHandle[]
						{
							coroutineHandle
						});
					}
				}
			}
			return num;
		}

		public int PauseCoroutinesOnInstance(CoroutineHandle handle)
		{
			int num = 0;
			if (this._handleToIndex.ContainsKey(handle) && !this.CoindexIsNull(this._handleToIndex[handle]) && !this.SetPause(this._handleToIndex[handle], true))
			{
				num++;
			}
			if (Timing.Links.ContainsKey(handle))
			{
				HashSet<CoroutineHandle> hashSet = Timing.Links[handle];
				Timing.Links.Remove(handle);
				foreach (CoroutineHandle coroutineHandle in hashSet)
				{
					num += Timing.PauseCoroutines(new CoroutineHandle[]
					{
						coroutineHandle
					});
				}
				Timing.Links.Add(handle, hashSet);
			}
			return num;
		}

		public static int PauseCoroutines(params CoroutineHandle[] handles)
		{
			int num = 0;
			for (int i = 0; i < handles.Length; i++)
			{
				num += ((Timing.ActiveInstances[(int)handles[i].Key] != null) ? Timing.GetInstance(handles[i].Key).PauseCoroutinesOnInstance(handles[i]) : 0);
			}
			return num;
		}

		public static int PauseCoroutines(GameObject gameObj)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.PauseCoroutinesOnInstance(gameObj);
			}
			return 0;
		}

		public int PauseCoroutinesOnInstance(GameObject gameObj)
		{
			if (!(gameObj == null))
			{
				return this.PauseCoroutinesOnInstance(gameObj.GetInstanceID());
			}
			return 0;
		}

		public static int PauseCoroutines(int layer)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.PauseCoroutinesOnInstance(layer);
			}
			return 0;
		}

		public int PauseCoroutinesOnInstance(int layer)
		{
			if (!this._layeredProcesses.ContainsKey(layer))
			{
				return 0;
			}
			int num = 0;
			HashSet<CoroutineHandle>.Enumerator enumerator = this._layeredProcesses[layer].GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!this.CoindexIsNull(this._handleToIndex[enumerator.Current]) && !this.SetPause(this._handleToIndex[enumerator.Current], true))
				{
					num++;
				}
				if (Timing.Links.ContainsKey(enumerator.Current))
				{
					HashSet<CoroutineHandle> hashSet = Timing.Links[enumerator.Current];
					Timing.Links.Remove(enumerator.Current);
					foreach (CoroutineHandle coroutineHandle in hashSet)
					{
						num += Timing.PauseCoroutines(new CoroutineHandle[]
						{
							coroutineHandle
						});
					}
					Timing.Links.Add(enumerator.Current, hashSet);
				}
			}
			return num;
		}

		public static int PauseCoroutines(string tag)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.PauseCoroutinesOnInstance(tag);
			}
			return 0;
		}

		public int PauseCoroutinesOnInstance(string tag)
		{
			if (tag == null || !this._taggedProcesses.ContainsKey(tag))
			{
				return 0;
			}
			int num = 0;
			HashSet<CoroutineHandle>.Enumerator enumerator = this._taggedProcesses[tag].GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!this.CoindexIsNull(this._handleToIndex[enumerator.Current]) && !this.SetPause(this._handleToIndex[enumerator.Current], true))
				{
					num++;
				}
				if (Timing.Links.ContainsKey(enumerator.Current))
				{
					HashSet<CoroutineHandle> hashSet = Timing.Links[enumerator.Current];
					Timing.Links.Remove(enumerator.Current);
					foreach (CoroutineHandle coroutineHandle in hashSet)
					{
						num += Timing.PauseCoroutines(new CoroutineHandle[]
						{
							coroutineHandle
						});
					}
					Timing.Links.Add(enumerator.Current, hashSet);
				}
			}
			return num;
		}

		public static int PauseCoroutines(GameObject gameObj, string tag)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.PauseCoroutinesOnInstance(gameObj.GetInstanceID(), tag);
			}
			return 0;
		}

		public int PauseCoroutinesOnInstance(GameObject gameObj, string tag)
		{
			if (!(gameObj == null))
			{
				return this.PauseCoroutinesOnInstance(gameObj.GetInstanceID(), tag);
			}
			return 0;
		}

		public static int PauseCoroutines(int layer, string tag)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.PauseCoroutinesOnInstance(layer, tag);
			}
			return 0;
		}

		public int PauseCoroutinesOnInstance(int layer, string tag)
		{
			if (tag == null)
			{
				return this.PauseCoroutinesOnInstance(layer);
			}
			if (!this._taggedProcesses.ContainsKey(tag) || !this._layeredProcesses.ContainsKey(layer))
			{
				return 0;
			}
			int num = 0;
			HashSet<CoroutineHandle>.Enumerator enumerator = this._taggedProcesses[tag].GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (this._processLayers.ContainsKey(enumerator.Current) && this._processLayers[enumerator.Current] == layer && !this.CoindexIsNull(this._handleToIndex[enumerator.Current]))
				{
					if (!this.SetPause(this._handleToIndex[enumerator.Current], true))
					{
						num++;
					}
					if (Timing.Links.ContainsKey(enumerator.Current))
					{
						HashSet<CoroutineHandle> hashSet = Timing.Links[enumerator.Current];
						Timing.Links.Remove(enumerator.Current);
						foreach (CoroutineHandle coroutineHandle in hashSet)
						{
							num += Timing.PauseCoroutines(new CoroutineHandle[]
							{
								coroutineHandle
							});
						}
						Timing.Links.Add(enumerator.Current, hashSet);
					}
				}
			}
			return num;
		}

		public static int ResumeCoroutines()
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.ResumeCoroutinesOnInstance();
			}
			return 0;
		}

		public int ResumeCoroutinesOnInstance()
		{
			int num = 0;
			Timing.ProcessIndex processIndex;
			processIndex.i = 0;
			processIndex.seg = Segment.Update;
			while (processIndex.i < this._nextUpdateProcessSlot)
			{
				if (this.UpdatePaused[processIndex.i] && this.UpdateProcesses[processIndex.i] != null)
				{
					this.UpdatePaused[processIndex.i] = false;
					num++;
				}
				processIndex.i++;
			}
			processIndex.i = 0;
			processIndex.seg = Segment.LateUpdate;
			while (processIndex.i < this._nextLateUpdateProcessSlot)
			{
				if (this.LateUpdatePaused[processIndex.i] && this.LateUpdateProcesses[processIndex.i] != null)
				{
					this.LateUpdatePaused[processIndex.i] = false;
					num++;
				}
				processIndex.i++;
			}
			processIndex.i = 0;
			processIndex.seg = Segment.FixedUpdate;
			while (processIndex.i < this._nextFixedUpdateProcessSlot)
			{
				if (this.FixedUpdatePaused[processIndex.i] && this.FixedUpdateProcesses[processIndex.i] != null)
				{
					this.FixedUpdatePaused[processIndex.i] = false;
					num++;
				}
				processIndex.i++;
			}
			processIndex.i = 0;
			processIndex.seg = Segment.SlowUpdate;
			while (processIndex.i < this._nextSlowUpdateProcessSlot)
			{
				if (this.SlowUpdatePaused[processIndex.i] && this.SlowUpdateProcesses[processIndex.i] != null)
				{
					this.SlowUpdatePaused[processIndex.i] = false;
					num++;
				}
				processIndex.i++;
			}
			processIndex.i = 0;
			processIndex.seg = Segment.RealtimeUpdate;
			while (processIndex.i < this._nextRealtimeUpdateProcessSlot)
			{
				if (this.RealtimeUpdatePaused[processIndex.i] && this.RealtimeUpdateProcesses[processIndex.i] != null)
				{
					this.RealtimeUpdatePaused[processIndex.i] = false;
					num++;
				}
				processIndex.i++;
			}
			processIndex.i = 0;
			processIndex.seg = Segment.EditorUpdate;
			while (processIndex.i < this._nextEditorUpdateProcessSlot)
			{
				if (this.EditorUpdatePaused[processIndex.i] && this.EditorUpdateProcesses[processIndex.i] != null)
				{
					this.EditorUpdatePaused[processIndex.i] = false;
					num++;
				}
				processIndex.i++;
			}
			processIndex.i = 0;
			processIndex.seg = Segment.EditorSlowUpdate;
			while (processIndex.i < this._nextEditorSlowUpdateProcessSlot)
			{
				if (this.EditorSlowUpdatePaused[processIndex.i] && this.EditorSlowUpdateProcesses[processIndex.i] != null)
				{
					this.EditorSlowUpdatePaused[processIndex.i] = false;
					num++;
				}
				processIndex.i++;
			}
			processIndex.i = 0;
			processIndex.seg = Segment.EndOfFrame;
			while (processIndex.i < this._nextEndOfFrameProcessSlot)
			{
				if (this.EndOfFramePaused[processIndex.i] && this.EndOfFrameProcesses[processIndex.i] != null)
				{
					this.EndOfFramePaused[processIndex.i] = false;
					num++;
				}
				processIndex.i++;
			}
			processIndex.i = 0;
			processIndex.seg = Segment.ManualTimeframe;
			while (processIndex.i < this._nextManualTimeframeProcessSlot)
			{
				if (this.ManualTimeframePaused[processIndex.i] && this.ManualTimeframeProcesses[processIndex.i] != null)
				{
					this.ManualTimeframePaused[processIndex.i] = false;
					num++;
				}
				processIndex.i++;
			}
			Dictionary<CoroutineHandle, HashSet<CoroutineHandle>>.Enumerator enumerator = Timing.Links.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Dictionary<CoroutineHandle, Timing.ProcessIndex> handleToIndex = this._handleToIndex;
				KeyValuePair<CoroutineHandle, HashSet<CoroutineHandle>> keyValuePair = enumerator.Current;
				if (handleToIndex.ContainsKey(keyValuePair.Key))
				{
					keyValuePair = enumerator.Current;
					foreach (CoroutineHandle coroutineHandle in keyValuePair.Value)
					{
						num += Timing.ResumeCoroutines(new CoroutineHandle[]
						{
							coroutineHandle
						});
					}
				}
			}
			return num;
		}

		public static int ResumeCoroutines(params CoroutineHandle[] handles)
		{
			int num = 0;
			for (int i = 0; i < handles.Length; i++)
			{
				num += ((Timing.ActiveInstances[(int)handles[i].Key] != null) ? Timing.GetInstance(handles[i].Key).ResumeCoroutinesOnInstance(handles[i]) : 0);
			}
			return num;
		}

		public int ResumeCoroutinesOnInstance(CoroutineHandle handle)
		{
			int num = 0;
			if (this._handleToIndex.ContainsKey(handle) && !this.CoindexIsNull(this._handleToIndex[handle]) && this.SetPause(this._handleToIndex[handle], false))
			{
				num++;
			}
			if (Timing.Links.ContainsKey(handle))
			{
				HashSet<CoroutineHandle> hashSet = Timing.Links[handle];
				Timing.Links.Remove(handle);
				foreach (CoroutineHandle coroutineHandle in hashSet)
				{
					num += Timing.ResumeCoroutines(new CoroutineHandle[]
					{
						coroutineHandle
					});
				}
				Timing.Links.Add(handle, hashSet);
			}
			return num;
		}

		public int ResumeCoroutinesOnInstance(IEnumerable<CoroutineHandle> handles)
		{
			int result = 0;
			IEnumerator<CoroutineHandle> enumerator = handles.GetEnumerator();
			while (!enumerator.MoveNext())
			{
				this.ResumeCoroutinesOnInstance(enumerator.Current);
			}
			return result;
		}

		public static int ResumeCoroutines(GameObject gameObj)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.ResumeCoroutinesOnInstance(gameObj.GetInstanceID());
			}
			return 0;
		}

		public int ResumeCoroutinesOnInstance(GameObject gameObj)
		{
			if (!(gameObj == null))
			{
				return this.ResumeCoroutinesOnInstance(gameObj.GetInstanceID());
			}
			return 0;
		}

		public static int ResumeCoroutines(int layer)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.ResumeCoroutinesOnInstance(layer);
			}
			return 0;
		}

		public int ResumeCoroutinesOnInstance(int layer)
		{
			if (!this._layeredProcesses.ContainsKey(layer))
			{
				return 0;
			}
			int num = 0;
			HashSet<CoroutineHandle>.Enumerator enumerator = this._layeredProcesses[layer].GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!this.CoindexIsNull(this._handleToIndex[enumerator.Current]) && this.SetPause(this._handleToIndex[enumerator.Current], false))
				{
					num++;
				}
				if (Timing.Links.ContainsKey(enumerator.Current))
				{
					HashSet<CoroutineHandle> hashSet = Timing.Links[enumerator.Current];
					Timing.Links.Remove(enumerator.Current);
					foreach (CoroutineHandle coroutineHandle in hashSet)
					{
						num += Timing.ResumeCoroutines(new CoroutineHandle[]
						{
							coroutineHandle
						});
					}
					Timing.Links.Add(enumerator.Current, hashSet);
				}
			}
			return num;
		}

		public static int ResumeCoroutines(string tag)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.ResumeCoroutinesOnInstance(tag);
			}
			return 0;
		}

		public int ResumeCoroutinesOnInstance(string tag)
		{
			if (tag == null || !this._taggedProcesses.ContainsKey(tag))
			{
				return 0;
			}
			int num = 0;
			HashSet<CoroutineHandle>.Enumerator enumerator = this._taggedProcesses[tag].GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!this.CoindexIsNull(this._handleToIndex[enumerator.Current]) && this.SetPause(this._handleToIndex[enumerator.Current], false))
				{
					num++;
				}
				if (Timing.Links.ContainsKey(enumerator.Current))
				{
					HashSet<CoroutineHandle> hashSet = Timing.Links[enumerator.Current];
					Timing.Links.Remove(enumerator.Current);
					foreach (CoroutineHandle coroutineHandle in hashSet)
					{
						num += Timing.ResumeCoroutines(new CoroutineHandle[]
						{
							coroutineHandle
						});
					}
					Timing.Links.Add(enumerator.Current, hashSet);
				}
			}
			return num;
		}

		public static int ResumeCoroutines(GameObject gameObj, string tag)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.ResumeCoroutinesOnInstance(gameObj.GetInstanceID(), tag);
			}
			return 0;
		}

		public int ResumeCoroutinesOnInstance(GameObject gameObj, string tag)
		{
			if (!(gameObj == null))
			{
				return this.ResumeCoroutinesOnInstance(gameObj.GetInstanceID(), tag);
			}
			return 0;
		}

		public static int ResumeCoroutines(int layer, string tag)
		{
			if (!(Timing._instance == null))
			{
				return Timing._instance.ResumeCoroutinesOnInstance(layer, tag);
			}
			return 0;
		}

		public int ResumeCoroutinesOnInstance(int layer, string tag)
		{
			if (tag == null)
			{
				return this.ResumeCoroutinesOnInstance(layer);
			}
			if (!this._layeredProcesses.ContainsKey(layer) || !this._taggedProcesses.ContainsKey(tag))
			{
				return 0;
			}
			int num = 0;
			HashSet<CoroutineHandle>.Enumerator enumerator = this._taggedProcesses[tag].GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!this.CoindexIsNull(this._handleToIndex[enumerator.Current]) && this._layeredProcesses[layer].Contains(enumerator.Current))
				{
					if (this.SetPause(this._handleToIndex[enumerator.Current], false))
					{
						num++;
					}
					if (Timing.Links.ContainsKey(enumerator.Current))
					{
						HashSet<CoroutineHandle> hashSet = Timing.Links[enumerator.Current];
						Timing.Links.Remove(enumerator.Current);
						foreach (CoroutineHandle coroutineHandle in hashSet)
						{
							num += Timing.ResumeCoroutines(new CoroutineHandle[]
							{
								coroutineHandle
							});
						}
						Timing.Links.Add(enumerator.Current, hashSet);
					}
				}
			}
			return num;
		}

		public static string GetTag(CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			if (!(instance != null) || !instance._handleToIndex.ContainsKey(handle) || !instance._processTags.ContainsKey(handle))
			{
				return null;
			}
			return instance._processTags[handle];
		}

		public static int? GetLayer(CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			if (!(instance != null) || !instance._handleToIndex.ContainsKey(handle) || !instance._processLayers.ContainsKey(handle))
			{
				return null;
			}
			return new int?(instance._processLayers[handle]);
		}

		public static string GetDebugName(CoroutineHandle handle)
		{
			if (handle.Key == 0)
			{
				return "Uninitialized handle";
			}
			Timing instance = Timing.GetInstance(handle.Key);
			if (instance == null)
			{
				return "Invalid handle";
			}
			if (!instance._handleToIndex.ContainsKey(handle))
			{
				return "Expired coroutine";
			}
			return instance.CoindexPeek(instance._handleToIndex[handle]).ToString();
		}

		public static Segment GetSegment(CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			if (!(instance != null) || !instance._handleToIndex.ContainsKey(handle))
			{
				return Segment.Invalid;
			}
			return instance._handleToIndex[handle].seg;
		}

		public static bool SetTag(CoroutineHandle handle, string newTag, bool overwriteExisting = true)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			if (instance == null || !instance._handleToIndex.ContainsKey(handle) || instance.CoindexIsNull(instance._handleToIndex[handle]) || (!overwriteExisting && instance._processTags.ContainsKey(handle)))
			{
				return false;
			}
			instance.RemoveTagOnInstance(handle);
			instance.AddTagOnInstance(newTag, handle);
			return true;
		}

		public static bool SetLayer(CoroutineHandle handle, int newLayer, bool overwriteExisting = true)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			if (instance == null || !instance._handleToIndex.ContainsKey(handle) || instance.CoindexIsNull(instance._handleToIndex[handle]) || (!overwriteExisting && instance._processLayers.ContainsKey(handle)))
			{
				return false;
			}
			instance.RemoveLayerOnInstance(handle);
			instance.AddLayerOnInstance(newLayer, handle);
			return true;
		}

		public static bool SetSegment(CoroutineHandle handle, Segment newSegment)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			if (instance == null || !instance._handleToIndex.ContainsKey(handle) || instance.CoindexIsNull(instance._handleToIndex[handle]))
			{
				return false;
			}
			Timing.ProcessIndex processIndex = instance._handleToIndex[handle];
			IEnumerator<float> enumerator = instance.CoindexExtract(processIndex);
			bool newHeldState = instance.CoindexIsHeld(processIndex);
			bool newPausedState = instance.CoindexIsPaused(processIndex);
			if (enumerator.Current > instance.GetSegmentTime(processIndex.seg))
			{
				enumerator = instance._InjectDelay(enumerator, enumerator.Current - instance.GetSegmentTime(processIndex.seg));
			}
			instance.RunCoroutineInternal(enumerator, newSegment, 0, false, null, handle, false);
			processIndex = instance._handleToIndex[handle];
			instance.SetHeld(processIndex, newHeldState);
			instance.SetPause(processIndex, newPausedState);
			return true;
		}

		public static bool RemoveTag(CoroutineHandle handle)
		{
			return Timing.SetTag(handle, null, true);
		}

		public static bool RemoveLayer(CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			if (instance == null || !instance._handleToIndex.ContainsKey(handle) || instance.CoindexIsNull(instance._handleToIndex[handle]))
			{
				return false;
			}
			instance.RemoveLayerOnInstance(handle);
			return true;
		}

		public static bool IsRunning(CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			return instance != null && instance._handleToIndex.ContainsKey(handle) && !instance.CoindexIsNull(instance._handleToIndex[handle]);
		}

		public static bool IsAliveAndPaused(CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			return instance != null && instance._handleToIndex.ContainsKey(handle) && !instance.CoindexIsNull(instance._handleToIndex[handle]) && instance.CoindexIsPaused(instance._handleToIndex[handle]);
		}

		private void AddTagOnInstance(string tag, CoroutineHandle handle)
		{
			this._processTags.Add(handle, tag);
			if (this._taggedProcesses.ContainsKey(tag))
			{
				this._taggedProcesses[tag].Add(handle);
				return;
			}
			this._taggedProcesses.Add(tag, new HashSet<CoroutineHandle>
			{
				handle
			});
		}

		private void AddLayerOnInstance(int layer, CoroutineHandle handle)
		{
			this._processLayers.Add(handle, layer);
			if (this._layeredProcesses.ContainsKey(layer))
			{
				this._layeredProcesses[layer].Add(handle);
				return;
			}
			this._layeredProcesses.Add(layer, new HashSet<CoroutineHandle>
			{
				handle
			});
		}

		private void RemoveTagOnInstance(CoroutineHandle handle)
		{
			if (this._processTags.ContainsKey(handle))
			{
				if (this._taggedProcesses[this._processTags[handle]].Count > 1)
				{
					this._taggedProcesses[this._processTags[handle]].Remove(handle);
				}
				else
				{
					this._taggedProcesses.Remove(this._processTags[handle]);
				}
				this._processTags.Remove(handle);
			}
		}

		private void RemoveLayerOnInstance(CoroutineHandle handle)
		{
			if (this._processLayers.ContainsKey(handle))
			{
				if (this._layeredProcesses[this._processLayers[handle]].Count > 1)
				{
					this._layeredProcesses[this._processLayers[handle]].Remove(handle);
				}
				else
				{
					this._layeredProcesses.Remove(this._processLayers[handle]);
				}
				this._processLayers.Remove(handle);
			}
		}

		private void RemoveGraffiti(CoroutineHandle handle)
		{
			if (this._processLayers.ContainsKey(handle))
			{
				if (this._layeredProcesses[this._processLayers[handle]].Count > 1)
				{
					this._layeredProcesses[this._processLayers[handle]].Remove(handle);
				}
				else
				{
					this._layeredProcesses.Remove(this._processLayers[handle]);
				}
				this._processLayers.Remove(handle);
			}
			if (this._processTags.ContainsKey(handle))
			{
				if (this._taggedProcesses[this._processTags[handle]].Count > 1)
				{
					this._taggedProcesses[this._processTags[handle]].Remove(handle);
				}
				else
				{
					this._taggedProcesses.Remove(this._processTags[handle]);
				}
				this._processTags.Remove(handle);
			}
		}

		private IEnumerator<float> CoindexExtract(Timing.ProcessIndex coindex)
		{
			switch (coindex.seg)
			{
				case Segment.Update:
					{
						IEnumerator<float> result = this.UpdateProcesses[coindex.i];
						this.UpdateProcesses[coindex.i] = null;
						return result;
					}
				case Segment.FixedUpdate:
					{
						IEnumerator<float> result2 = this.FixedUpdateProcesses[coindex.i];
						this.FixedUpdateProcesses[coindex.i] = null;
						return result2;
					}
				case Segment.LateUpdate:
					{
						IEnumerator<float> result3 = this.LateUpdateProcesses[coindex.i];
						this.LateUpdateProcesses[coindex.i] = null;
						return result3;
					}
				case Segment.SlowUpdate:
					{
						IEnumerator<float> result4 = this.SlowUpdateProcesses[coindex.i];
						this.SlowUpdateProcesses[coindex.i] = null;
						return result4;
					}
				case Segment.RealtimeUpdate:
					{
						IEnumerator<float> result5 = this.RealtimeUpdateProcesses[coindex.i];
						this.RealtimeUpdateProcesses[coindex.i] = null;
						return result5;
					}
				case Segment.EditorUpdate:
					{
						IEnumerator<float> result6 = this.EditorUpdateProcesses[coindex.i];
						this.EditorUpdateProcesses[coindex.i] = null;
						return result6;
					}
				case Segment.EditorSlowUpdate:
					{
						IEnumerator<float> result7 = this.EditorSlowUpdateProcesses[coindex.i];
						this.EditorSlowUpdateProcesses[coindex.i] = null;
						return result7;
					}
				case Segment.EndOfFrame:
					{
						IEnumerator<float> result8 = this.EndOfFrameProcesses[coindex.i];
						this.EndOfFrameProcesses[coindex.i] = null;
						return result8;
					}
				case Segment.ManualTimeframe:
					{
						IEnumerator<float> result9 = this.ManualTimeframeProcesses[coindex.i];
						this.ManualTimeframeProcesses[coindex.i] = null;
						return result9;
					}
				default:
					return null;
			}
		}

		private bool CoindexIsNull(Timing.ProcessIndex coindex)
		{
			switch (coindex.seg)
			{
				case Segment.Update:
					return this.UpdateProcesses[coindex.i] == null;
				case Segment.FixedUpdate:
					return this.FixedUpdateProcesses[coindex.i] == null;
				case Segment.LateUpdate:
					return this.LateUpdateProcesses[coindex.i] == null;
				case Segment.SlowUpdate:
					return this.SlowUpdateProcesses[coindex.i] == null;
				case Segment.RealtimeUpdate:
					return this.RealtimeUpdateProcesses[coindex.i] == null;
				case Segment.EditorUpdate:
					return this.EditorUpdateProcesses[coindex.i] == null;
				case Segment.EditorSlowUpdate:
					return this.EditorSlowUpdateProcesses[coindex.i] == null;
				case Segment.EndOfFrame:
					return this.EndOfFrameProcesses[coindex.i] == null;
				case Segment.ManualTimeframe:
					return this.ManualTimeframeProcesses[coindex.i] == null;
				default:
					return true;
			}
		}

		private IEnumerator<float> CoindexPeek(Timing.ProcessIndex coindex)
		{
			switch (coindex.seg)
			{
				case Segment.Update:
					return this.UpdateProcesses[coindex.i];
				case Segment.FixedUpdate:
					return this.FixedUpdateProcesses[coindex.i];
				case Segment.LateUpdate:
					return this.LateUpdateProcesses[coindex.i];
				case Segment.SlowUpdate:
					return this.SlowUpdateProcesses[coindex.i];
				case Segment.RealtimeUpdate:
					return this.RealtimeUpdateProcesses[coindex.i];
				case Segment.EditorUpdate:
					return this.EditorUpdateProcesses[coindex.i];
				case Segment.EditorSlowUpdate:
					return this.EditorSlowUpdateProcesses[coindex.i];
				case Segment.EndOfFrame:
					return this.EndOfFrameProcesses[coindex.i];
				case Segment.ManualTimeframe:
					return this.ManualTimeframeProcesses[coindex.i];
				default:
					return null;
			}
		}

		private bool Nullify(CoroutineHandle handle)
		{
			return this.Nullify(this._handleToIndex[handle]);
		}

		private bool Nullify(Timing.ProcessIndex coindex)
		{
			switch (coindex.seg)
			{
				case Segment.Update:
					{
						bool result = this.UpdateProcesses[coindex.i] != null;
						this.UpdateProcesses[coindex.i] = null;
						return result;
					}
				case Segment.FixedUpdate:
					{
						bool result2 = this.FixedUpdateProcesses[coindex.i] != null;
						this.FixedUpdateProcesses[coindex.i] = null;
						return result2;
					}
				case Segment.LateUpdate:
					{
						bool result3 = this.LateUpdateProcesses[coindex.i] != null;
						this.LateUpdateProcesses[coindex.i] = null;
						return result3;
					}
				case Segment.SlowUpdate:
					{
						bool result4 = this.SlowUpdateProcesses[coindex.i] != null;
						this.SlowUpdateProcesses[coindex.i] = null;
						return result4;
					}
				case Segment.RealtimeUpdate:
					{
						bool result5 = this.RealtimeUpdateProcesses[coindex.i] != null;
						this.RealtimeUpdateProcesses[coindex.i] = null;
						return result5;
					}
				case Segment.EditorUpdate:
					{
						bool result6 = this.UpdateProcesses[coindex.i] != null;
						this.EditorUpdateProcesses[coindex.i] = null;
						return result6;
					}
				case Segment.EditorSlowUpdate:
					{
						bool result7 = this.EditorSlowUpdateProcesses[coindex.i] != null;
						this.EditorSlowUpdateProcesses[coindex.i] = null;
						return result7;
					}
				case Segment.EndOfFrame:
					{
						bool result8 = this.EndOfFrameProcesses[coindex.i] != null;
						this.EndOfFrameProcesses[coindex.i] = null;
						return result8;
					}
				case Segment.ManualTimeframe:
					{
						bool result9 = this.ManualTimeframeProcesses[coindex.i] != null;
						this.ManualTimeframeProcesses[coindex.i] = null;
						return result9;
					}
				default:
					return false;
			}
		}

		private bool SetPause(Timing.ProcessIndex coindex, bool newPausedState)
		{
			if (this.CoindexPeek(coindex) == null)
			{
				return false;
			}
			switch (coindex.seg)
			{
				case Segment.Update:
					{
						bool result = this.UpdatePaused[coindex.i];
						this.UpdatePaused[coindex.i] = newPausedState;
						if (newPausedState && this.UpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.UpdateProcesses[coindex.i] = this._InjectDelay(this.UpdateProcesses[coindex.i], this.UpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result;
					}
				case Segment.FixedUpdate:
					{
						bool result2 = this.FixedUpdatePaused[coindex.i];
						this.FixedUpdatePaused[coindex.i] = newPausedState;
						if (newPausedState && this.FixedUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.FixedUpdateProcesses[coindex.i] = this._InjectDelay(this.FixedUpdateProcesses[coindex.i], this.FixedUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result2;
					}
				case Segment.LateUpdate:
					{
						bool result3 = this.LateUpdatePaused[coindex.i];
						this.LateUpdatePaused[coindex.i] = newPausedState;
						if (newPausedState && this.LateUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.LateUpdateProcesses[coindex.i] = this._InjectDelay(this.LateUpdateProcesses[coindex.i], this.LateUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result3;
					}
				case Segment.SlowUpdate:
					{
						bool result4 = this.SlowUpdatePaused[coindex.i];
						this.SlowUpdatePaused[coindex.i] = newPausedState;
						if (newPausedState && this.SlowUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.SlowUpdateProcesses[coindex.i] = this._InjectDelay(this.SlowUpdateProcesses[coindex.i], this.SlowUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result4;
					}
				case Segment.RealtimeUpdate:
					{
						bool result5 = this.RealtimeUpdatePaused[coindex.i];
						this.RealtimeUpdatePaused[coindex.i] = newPausedState;
						if (newPausedState && this.RealtimeUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.RealtimeUpdateProcesses[coindex.i] = this._InjectDelay(this.RealtimeUpdateProcesses[coindex.i], this.RealtimeUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result5;
					}
				case Segment.EditorUpdate:
					{
						bool result6 = this.EditorUpdatePaused[coindex.i];
						this.EditorUpdatePaused[coindex.i] = newPausedState;
						if (newPausedState && this.EditorUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.EditorUpdateProcesses[coindex.i] = this._InjectDelay(this.EditorUpdateProcesses[coindex.i], this.EditorUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result6;
					}
				case Segment.EditorSlowUpdate:
					{
						bool result7 = this.EditorSlowUpdatePaused[coindex.i];
						this.EditorSlowUpdatePaused[coindex.i] = newPausedState;
						if (newPausedState && this.EditorSlowUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.EditorSlowUpdateProcesses[coindex.i] = this._InjectDelay(this.EditorSlowUpdateProcesses[coindex.i], this.EditorSlowUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result7;
					}
				case Segment.EndOfFrame:
					{
						bool result8 = this.EndOfFramePaused[coindex.i];
						this.EndOfFramePaused[coindex.i] = newPausedState;
						if (newPausedState && this.EndOfFrameProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.EndOfFrameProcesses[coindex.i] = this._InjectDelay(this.EndOfFrameProcesses[coindex.i], this.EndOfFrameProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result8;
					}
				case Segment.ManualTimeframe:
					{
						bool result9 = this.ManualTimeframePaused[coindex.i];
						this.ManualTimeframePaused[coindex.i] = newPausedState;
						if (newPausedState && this.ManualTimeframeProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.ManualTimeframeProcesses[coindex.i] = this._InjectDelay(this.ManualTimeframeProcesses[coindex.i], this.ManualTimeframeProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result9;
					}
				default:
					return false;
			}
		}

		private bool SetHeld(Timing.ProcessIndex coindex, bool newHeldState)
		{
			if (this.CoindexPeek(coindex) == null)
			{
				return false;
			}
			switch (coindex.seg)
			{
				case Segment.Update:
					{
						bool result = this.UpdateHeld[coindex.i];
						this.UpdateHeld[coindex.i] = newHeldState;
						if (newHeldState && this.UpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.UpdateProcesses[coindex.i] = this._InjectDelay(this.UpdateProcesses[coindex.i], this.UpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result;
					}
				case Segment.FixedUpdate:
					{
						bool result2 = this.FixedUpdateHeld[coindex.i];
						this.FixedUpdateHeld[coindex.i] = newHeldState;
						if (newHeldState && this.FixedUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.FixedUpdateProcesses[coindex.i] = this._InjectDelay(this.FixedUpdateProcesses[coindex.i], this.FixedUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result2;
					}
				case Segment.LateUpdate:
					{
						bool result3 = this.LateUpdateHeld[coindex.i];
						this.LateUpdateHeld[coindex.i] = newHeldState;
						if (newHeldState && this.LateUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.LateUpdateProcesses[coindex.i] = this._InjectDelay(this.LateUpdateProcesses[coindex.i], this.LateUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result3;
					}
				case Segment.SlowUpdate:
					{
						bool result4 = this.SlowUpdateHeld[coindex.i];
						this.SlowUpdateHeld[coindex.i] = newHeldState;
						if (newHeldState && this.SlowUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.SlowUpdateProcesses[coindex.i] = this._InjectDelay(this.SlowUpdateProcesses[coindex.i], this.SlowUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result4;
					}
				case Segment.RealtimeUpdate:
					{
						bool result5 = this.RealtimeUpdateHeld[coindex.i];
						this.RealtimeUpdateHeld[coindex.i] = newHeldState;
						if (newHeldState && this.RealtimeUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.RealtimeUpdateProcesses[coindex.i] = this._InjectDelay(this.RealtimeUpdateProcesses[coindex.i], this.RealtimeUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result5;
					}
				case Segment.EditorUpdate:
					{
						bool result6 = this.EditorUpdateHeld[coindex.i];
						this.EditorUpdateHeld[coindex.i] = newHeldState;
						if (newHeldState && this.EditorUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.EditorUpdateProcesses[coindex.i] = this._InjectDelay(this.EditorUpdateProcesses[coindex.i], this.EditorUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result6;
					}
				case Segment.EditorSlowUpdate:
					{
						bool result7 = this.EditorSlowUpdateHeld[coindex.i];
						this.EditorSlowUpdateHeld[coindex.i] = newHeldState;
						if (newHeldState && this.EditorSlowUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.EditorSlowUpdateProcesses[coindex.i] = this._InjectDelay(this.EditorSlowUpdateProcesses[coindex.i], this.EditorSlowUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result7;
					}
				case Segment.EndOfFrame:
					{
						bool result8 = this.EndOfFrameHeld[coindex.i];
						this.EndOfFrameHeld[coindex.i] = newHeldState;
						if (newHeldState && this.EndOfFrameProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.EndOfFrameProcesses[coindex.i] = this._InjectDelay(this.EndOfFrameProcesses[coindex.i], this.EndOfFrameProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result8;
					}
				case Segment.ManualTimeframe:
					{
						bool result9 = this.ManualTimeframeHeld[coindex.i];
						this.ManualTimeframeHeld[coindex.i] = newHeldState;
						if (newHeldState && this.ManualTimeframeProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
						{
							this.ManualTimeframeProcesses[coindex.i] = this._InjectDelay(this.ManualTimeframeProcesses[coindex.i], this.ManualTimeframeProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
						}
						return result9;
					}
				default:
					return false;
			}
		}

		private IEnumerator<float> CreateHold(Timing.ProcessIndex coindex, IEnumerator<float> coptr)
		{
			if (this.CoindexPeek(coindex) == null)
			{
				return null;
			}
			switch (coindex.seg)
			{
				case Segment.Update:
					this.UpdateHeld[coindex.i] = true;
					if (this.UpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
					{
						coptr = this._InjectDelay(this.UpdateProcesses[coindex.i], this.UpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
					}
					return coptr;
				case Segment.FixedUpdate:
					this.FixedUpdateHeld[coindex.i] = true;
					if (this.FixedUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
					{
						coptr = this._InjectDelay(this.FixedUpdateProcesses[coindex.i], this.FixedUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
					}
					return coptr;
				case Segment.LateUpdate:
					this.LateUpdateHeld[coindex.i] = true;
					if (this.LateUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
					{
						coptr = this._InjectDelay(this.LateUpdateProcesses[coindex.i], this.LateUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
					}
					return coptr;
				case Segment.SlowUpdate:
					this.SlowUpdateHeld[coindex.i] = true;
					if (this.SlowUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
					{
						coptr = this._InjectDelay(this.SlowUpdateProcesses[coindex.i], this.SlowUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
					}
					return coptr;
				case Segment.RealtimeUpdate:
					this.RealtimeUpdateHeld[coindex.i] = true;
					if (this.RealtimeUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
					{
						coptr = this._InjectDelay(this.RealtimeUpdateProcesses[coindex.i], this.RealtimeUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
					}
					return coptr;
				case Segment.EditorUpdate:
					this.EditorUpdateHeld[coindex.i] = true;
					if (this.EditorUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
					{
						coptr = this._InjectDelay(this.EditorUpdateProcesses[coindex.i], this.EditorUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
					}
					return coptr;
				case Segment.EditorSlowUpdate:
					this.EditorSlowUpdateHeld[coindex.i] = true;
					if (this.EditorSlowUpdateProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
					{
						coptr = this._InjectDelay(this.EditorSlowUpdateProcesses[coindex.i], this.EditorSlowUpdateProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
					}
					return coptr;
				case Segment.EndOfFrame:
					this.EndOfFrameHeld[coindex.i] = true;
					if (this.EndOfFrameProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
					{
						coptr = this._InjectDelay(this.EndOfFrameProcesses[coindex.i], this.EndOfFrameProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
					}
					return coptr;
				case Segment.ManualTimeframe:
					this.ManualTimeframeHeld[coindex.i] = true;
					if (this.ManualTimeframeProcesses[coindex.i].Current > this.GetSegmentTime(coindex.seg))
					{
						coptr = this._InjectDelay(this.ManualTimeframeProcesses[coindex.i], this.ManualTimeframeProcesses[coindex.i].Current - this.GetSegmentTime(coindex.seg));
					}
					return coptr;
				default:
					return coptr;
			}
		}

		private bool CoindexIsPaused(Timing.ProcessIndex coindex)
		{
			switch (coindex.seg)
			{
				case Segment.Update:
					return this.UpdatePaused[coindex.i];
				case Segment.FixedUpdate:
					return this.FixedUpdatePaused[coindex.i];
				case Segment.LateUpdate:
					return this.LateUpdatePaused[coindex.i];
				case Segment.SlowUpdate:
					return this.SlowUpdatePaused[coindex.i];
				case Segment.RealtimeUpdate:
					return this.RealtimeUpdatePaused[coindex.i];
				case Segment.EditorUpdate:
					return this.EditorUpdatePaused[coindex.i];
				case Segment.EditorSlowUpdate:
					return this.EditorSlowUpdatePaused[coindex.i];
				case Segment.EndOfFrame:
					return this.EndOfFramePaused[coindex.i];
				case Segment.ManualTimeframe:
					return this.ManualTimeframePaused[coindex.i];
				default:
					return false;
			}
		}

		private bool CoindexIsHeld(Timing.ProcessIndex coindex)
		{
			switch (coindex.seg)
			{
				case Segment.Update:
					return this.UpdateHeld[coindex.i];
				case Segment.FixedUpdate:
					return this.FixedUpdateHeld[coindex.i];
				case Segment.LateUpdate:
					return this.LateUpdateHeld[coindex.i];
				case Segment.SlowUpdate:
					return this.SlowUpdateHeld[coindex.i];
				case Segment.RealtimeUpdate:
					return this.RealtimeUpdateHeld[coindex.i];
				case Segment.EditorUpdate:
					return this.EditorUpdateHeld[coindex.i];
				case Segment.EditorSlowUpdate:
					return this.EditorSlowUpdateHeld[coindex.i];
				case Segment.EndOfFrame:
					return this.EndOfFrameHeld[coindex.i];
				case Segment.ManualTimeframe:
					return this.ManualTimeframeHeld[coindex.i];
				default:
					return false;
			}
		}

		private void CoindexReplace(Timing.ProcessIndex coindex, IEnumerator<float> replacement)
		{
			switch (coindex.seg)
			{
				case Segment.Update:
					this.UpdateProcesses[coindex.i] = replacement;
					return;
				case Segment.FixedUpdate:
					this.FixedUpdateProcesses[coindex.i] = replacement;
					return;
				case Segment.LateUpdate:
					this.LateUpdateProcesses[coindex.i] = replacement;
					return;
				case Segment.SlowUpdate:
					this.SlowUpdateProcesses[coindex.i] = replacement;
					return;
				case Segment.RealtimeUpdate:
					this.RealtimeUpdateProcesses[coindex.i] = replacement;
					return;
				case Segment.EditorUpdate:
					this.EditorUpdateProcesses[coindex.i] = replacement;
					return;
				case Segment.EditorSlowUpdate:
					this.EditorSlowUpdateProcesses[coindex.i] = replacement;
					return;
				case Segment.EndOfFrame:
					this.EndOfFrameProcesses[coindex.i] = replacement;
					return;
				case Segment.ManualTimeframe:
					this.ManualTimeframeProcesses[coindex.i] = replacement;
					return;
				default:
					return;
			}
		}

		public static float WaitUntilDone(IEnumerator<float> newCoroutine)
		{
			return Timing.WaitUntilDone(Timing.RunCoroutine(newCoroutine, Timing.CurrentCoroutine.Segment), true);
		}

		public static float WaitUntilDone(IEnumerator<float> newCoroutine, string tag)
		{
			return Timing.WaitUntilDone(Timing.RunCoroutine(newCoroutine, Timing.CurrentCoroutine.Segment, tag), true);
		}

		public static float WaitUntilDone(IEnumerator<float> newCoroutine, int layer)
		{
			return Timing.WaitUntilDone(Timing.RunCoroutine(newCoroutine, Timing.CurrentCoroutine.Segment, layer), true);
		}

		public static float WaitUntilDone(IEnumerator<float> newCoroutine, int layer, string tag)
		{
			return Timing.WaitUntilDone(Timing.RunCoroutine(newCoroutine, Timing.CurrentCoroutine.Segment, layer, tag), true);
		}

		public static float WaitUntilDone(IEnumerator<float> newCoroutine, Segment segment)
		{
			return Timing.WaitUntilDone(Timing.RunCoroutine(newCoroutine, segment), true);
		}

		public static float WaitUntilDone(IEnumerator<float> newCoroutine, Segment segment, string tag)
		{
			return Timing.WaitUntilDone(Timing.RunCoroutine(newCoroutine, segment, tag), true);
		}

		public static float WaitUntilDone(IEnumerator<float> newCoroutine, Segment segment, int layer)
		{
			return Timing.WaitUntilDone(Timing.RunCoroutine(newCoroutine, segment, layer), true);
		}

		public static float WaitUntilDone(IEnumerator<float> newCoroutine, Segment segment, int layer, string tag)
		{
			return Timing.WaitUntilDone(Timing.RunCoroutine(newCoroutine, segment, layer, tag), true);
		}

		public static float WaitUntilDone(CoroutineHandle otherCoroutine)
		{
			return Timing.WaitUntilDone(otherCoroutine, true);
		}

		public static float WaitUntilDone(CoroutineHandle otherCoroutine, bool warnOnIssue)
		{
			Timing instance = Timing.GetInstance(otherCoroutine.Key);
			if (!(instance != null) || !instance._handleToIndex.ContainsKey(otherCoroutine))
			{
				return 0f;
			}
			if (instance.CoindexIsNull(instance._handleToIndex[otherCoroutine]))
			{
				return 0f;
			}
			if (!instance._waitingTriggers.ContainsKey(otherCoroutine))
			{
				instance.CoindexReplace(instance._handleToIndex[otherCoroutine], instance._StartWhenDone(otherCoroutine, instance.CoindexPeek(instance._handleToIndex[otherCoroutine])));
				instance._waitingTriggers.Add(otherCoroutine, new HashSet<CoroutineHandle>());
			}
			if (instance.currentCoroutine == otherCoroutine)
			{
				return float.NegativeInfinity;
			}
			if (!instance.currentCoroutine.IsValid)
			{
				return float.NegativeInfinity;
			}
			instance._waitingTriggers[otherCoroutine].Add(instance.currentCoroutine);
			if (!instance._allWaiting.Contains(instance.currentCoroutine))
			{
				instance._allWaiting.Add(instance.currentCoroutine);
			}
			instance.SetHeld(instance._handleToIndex[instance.currentCoroutine], true);
			instance.SwapToLast(otherCoroutine, instance.currentCoroutine);
			return float.NaN;
		}

		public static void WaitForOtherHandles(CoroutineHandle handle, CoroutineHandle otherHandle, bool warnOnIssue = true)
		{
			if (!Timing.IsRunning(handle) || !Timing.IsRunning(otherHandle))
			{
				return;
			}
			if (handle == otherHandle)
			{
				return;
			}
			if (handle.Key != otherHandle.Key)
			{
				return;
			}
			Timing instance = Timing.GetInstance(handle.Key);
			if (instance != null && instance._handleToIndex.ContainsKey(handle) && instance._handleToIndex.ContainsKey(otherHandle) && !instance.CoindexIsNull(instance._handleToIndex[otherHandle]))
			{
				if (!instance._waitingTriggers.ContainsKey(otherHandle))
				{
					instance.CoindexReplace(instance._handleToIndex[otherHandle], instance._StartWhenDone(otherHandle, instance.CoindexPeek(instance._handleToIndex[otherHandle])));
					instance._waitingTriggers.Add(otherHandle, new HashSet<CoroutineHandle>());
				}
				instance._waitingTriggers[otherHandle].Add(handle);
				if (!instance._allWaiting.Contains(handle))
				{
					instance._allWaiting.Add(handle);
				}
				instance.SetHeld(instance._handleToIndex[handle], true);
				instance.SwapToLast(otherHandle, handle);
			}
		}

		public static void WaitForOtherHandles(CoroutineHandle handle, IEnumerable<CoroutineHandle> otherHandles, bool warnOnIssue = true)
		{
			if (!Timing.IsRunning(handle))
			{
				return;
			}
			Timing instance = Timing.GetInstance(handle.Key);
			IEnumerator<CoroutineHandle> enumerator = otherHandles.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (Timing.IsRunning(enumerator.Current) && !(handle == enumerator.Current))
				{
					byte key = handle.Key;
					CoroutineHandle coroutineHandle = enumerator.Current;
					if (key == coroutineHandle.Key)
					{
						if (!instance._waitingTriggers.ContainsKey(enumerator.Current))
						{
							instance.CoindexReplace(instance._handleToIndex[enumerator.Current], instance._StartWhenDone(enumerator.Current, instance.CoindexPeek(instance._handleToIndex[enumerator.Current])));
							instance._waitingTriggers.Add(enumerator.Current, new HashSet<CoroutineHandle>());
						}
						instance._waitingTriggers[enumerator.Current].Add(handle);
						if (!instance._allWaiting.Contains(handle))
						{
							instance._allWaiting.Add(handle);
						}
						instance.SetHeld(instance._handleToIndex[handle], true);
						instance.SwapToLast(enumerator.Current, handle);
					}
				}
			}
		}

		private void SwapToLast(CoroutineHandle firstHandle, CoroutineHandle lastHandle)
		{
			if (firstHandle.Key != lastHandle.Key)
			{
				return;
			}
			Timing.ProcessIndex processIndex = this._handleToIndex[firstHandle];
			Timing.ProcessIndex processIndex2 = this._handleToIndex[lastHandle];
			if (processIndex.seg != processIndex2.seg || processIndex.i <= processIndex2.i)
			{
				return;
			}
			IEnumerator<float> replacement = this.CoindexPeek(processIndex);
			this.CoindexReplace(processIndex, this.CoindexPeek(processIndex2));
			this.CoindexReplace(processIndex2, replacement);
			this._indexToHandle[processIndex] = lastHandle;
			this._indexToHandle[processIndex2] = firstHandle;
			this._handleToIndex[firstHandle] = processIndex2;
			this._handleToIndex[lastHandle] = processIndex;
			bool flag = this.SetPause(processIndex, this.CoindexIsPaused(processIndex2));
			this.SetPause(processIndex2, flag);
			flag = this.SetHeld(processIndex, this.CoindexIsHeld(processIndex2));
			this.SetHeld(processIndex2, flag);
			if (this._waitingTriggers.ContainsKey(lastHandle))
			{
				foreach (CoroutineHandle lastHandle2 in this._waitingTriggers[lastHandle])
				{
					this.SwapToLast(lastHandle, lastHandle2);
				}
			}
			if (this._allWaiting.Contains(firstHandle))
			{
				foreach (KeyValuePair<CoroutineHandle, HashSet<CoroutineHandle>> keyValuePair in this._waitingTriggers)
				{
					HashSet<CoroutineHandle>.Enumerator enumerator3 = keyValuePair.Value.GetEnumerator();
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current == firstHandle)
						{
							this.SwapToLast(enumerator3.Current, firstHandle);
						}
					}
				}
			}
		}

		private IEnumerator<float> _StartWhenDone(CoroutineHandle handle, IEnumerator<float> proc)
		{
			if (!this._waitingTriggers.ContainsKey(handle))
			{
				yield break;
			}
			try
			{
				if (proc.Current > this.localTime)
				{
					yield return proc.Current;
				}
				while (proc.MoveNext())
				{
					float num = proc.Current;
					yield return num;
				}
			}
			finally
			{
				this.CloseWaitingProcess(handle);
			}
			yield break;
		}

		private void CloseWaitingProcess(CoroutineHandle handle)
		{
			if (!this._waitingTriggers.ContainsKey(handle))
			{
				return;
			}
			HashSet<CoroutineHandle>.Enumerator enumerator = this._waitingTriggers[handle].GetEnumerator();
			this._waitingTriggers.Remove(handle);
			while (enumerator.MoveNext())
			{
				if (this._handleToIndex.ContainsKey(enumerator.Current) && !this.HandleIsInWaitingList(enumerator.Current))
				{
					this.SetHeld(this._handleToIndex[enumerator.Current], false);
					this._allWaiting.Remove(enumerator.Current);
				}
			}
		}

		private bool HandleIsInWaitingList(CoroutineHandle handle)
		{
			foreach (KeyValuePair<CoroutineHandle, HashSet<CoroutineHandle>> keyValuePair in this._waitingTriggers)
			{
				if (keyValuePair.Value.Contains(handle))
				{
					return true;
				}
			}
			return false;
		}

		private static IEnumerator<float> ReturnTmpRefForRepFunc(IEnumerator<float> coptr, CoroutineHandle handle)
		{
			return Timing._tmpRef as IEnumerator<float>;
		}

		public static float WaitUntilDone(AsyncOperation operation)
		{
			if (operation == null || operation.isDone)
			{
				return float.NaN;
			}
			CoroutineHandle currentCoroutine = Timing.CurrentCoroutine;
			Timing instance = Timing.GetInstance(Timing.CurrentCoroutine.Key);
			if (instance == null)
			{
				return float.NaN;
			}
			Timing._tmpRef = Timing._StartWhenDone(operation, instance.CoindexPeek(instance._handleToIndex[currentCoroutine]));
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.ReturnTmpRefForRepFunc);
			return float.NaN;
		}

		private static IEnumerator<float> _StartWhenDone(AsyncOperation operation, IEnumerator<float> pausedProc)
		{
			while (!operation.isDone)
			{
				yield return float.NegativeInfinity;
			}
			Timing._tmpRef = pausedProc;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.ReturnTmpRefForRepFunc);
			yield return float.NaN;
			yield break;
		}

		public static float WaitUntilDone(CustomYieldInstruction operation)
		{
			if (operation == null || !operation.keepWaiting)
			{
				return float.NaN;
			}
			CoroutineHandle currentCoroutine = Timing.CurrentCoroutine;
			Timing instance = Timing.GetInstance(Timing.CurrentCoroutine.Key);
			if (instance == null)
			{
				return float.NaN;
			}
			Timing._tmpRef = Timing._StartWhenDone(operation, instance.CoindexPeek(instance._handleToIndex[currentCoroutine]));
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.ReturnTmpRefForRepFunc);
			return float.NaN;
		}

		private static IEnumerator<float> _StartWhenDone(CustomYieldInstruction operation, IEnumerator<float> pausedProc)
		{
			while (operation.keepWaiting)
			{
				yield return float.NegativeInfinity;
			}
			Timing._tmpRef = pausedProc;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.ReturnTmpRefForRepFunc);
			yield return float.NaN;
			yield break;
		}

		public static float WaitUntilTrue(Func<bool> evaluatorFunc)
		{
			if (evaluatorFunc == null || evaluatorFunc())
			{
				return float.NaN;
			}
			Timing._tmpRef = evaluatorFunc;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.WaitUntilTrueHelper);
			return float.NaN;
		}

		private static IEnumerator<float> WaitUntilTrueHelper(IEnumerator<float> coptr, CoroutineHandle handle)
		{
			return Timing._StartWhenDone(Timing._tmpRef as Func<bool>, false, coptr);
		}

		public static float WaitUntilFalse(Func<bool> evaluatorFunc)
		{
			if (evaluatorFunc == null || !evaluatorFunc())
			{
				return float.NaN;
			}
			Timing._tmpRef = evaluatorFunc;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.WaitUntilFalseHelper);
			return float.NaN;
		}

		private static IEnumerator<float> WaitUntilFalseHelper(IEnumerator<float> coptr, CoroutineHandle handle)
		{
			return Timing._StartWhenDone(Timing._tmpRef as Func<bool>, true, coptr);
		}

		private static IEnumerator<float> _StartWhenDone(Func<bool> evaluatorFunc, bool continueOn, IEnumerator<float> pausedProc)
		{
			while (evaluatorFunc() == continueOn)
			{
				yield return float.NegativeInfinity;
			}
			Timing._tmpRef = pausedProc;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.ReturnTmpRefForRepFunc);
			yield return float.NaN;
			yield break;
		}

		private IEnumerator<float> _InjectDelay(IEnumerator<float> proc, float waitTime)
		{
			yield return this.WaitForSecondsOnInstance(waitTime);
			Timing._tmpRef = proc;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.ReturnTmpRefForRepFunc);
			yield return float.NaN;
			yield break;
		}

		public bool LockCoroutine(CoroutineHandle coroutine, CoroutineHandle key)
		{
			if (coroutine.Key != this._instanceID || key == default(CoroutineHandle) || key.Key != 0)
			{
				return false;
			}
			if (!this._waitingTriggers.ContainsKey(key))
			{
				this._waitingTriggers.Add(key, new HashSet<CoroutineHandle>
				{
					coroutine
				});
			}
			else
			{
				this._waitingTriggers[key].Add(coroutine);
			}
			this._allWaiting.Add(coroutine);
			this.SetHeld(this._handleToIndex[coroutine], true);
			return true;
		}

		public bool UnlockCoroutine(CoroutineHandle coroutine, CoroutineHandle key)
		{
			if (coroutine.Key != this._instanceID || key == default(CoroutineHandle) || !this._handleToIndex.ContainsKey(coroutine) || !this._waitingTriggers.ContainsKey(key))
			{
				return false;
			}
			if (this._waitingTriggers[key].Count == 1)
			{
				this._waitingTriggers.Remove(key);
			}
			else
			{
				this._waitingTriggers[key].Remove(coroutine);
			}
			if (!this.HandleIsInWaitingList(coroutine))
			{
				this.SetHeld(this._handleToIndex[coroutine], false);
				this._allWaiting.Remove(coroutine);
			}
			return true;
		}

		public static int LinkCoroutines(CoroutineHandle master, CoroutineHandle slave)
		{
			if (!Timing.IsRunning(slave) || !master.IsValid)
			{
				return 0;
			}
			if (!Timing.IsRunning(master))
			{
				Timing.KillCoroutines(new CoroutineHandle[]
				{
					slave
				});
				return 1;
			}
			if (!Timing.Links.ContainsKey(master))
			{
				Timing.Links.Add(master, new HashSet<CoroutineHandle>
				{
					slave
				});
				return 1;
			}
			if (!Timing.Links[master].Contains(slave))
			{
				Timing.Links[master].Add(slave);
				return 1;
			}
			return 0;
		}

		public static int UnlinkCoroutines(CoroutineHandle master, CoroutineHandle slave, bool twoWay = false)
		{
			int num = 0;
			if (Timing.Links.ContainsKey(master) && Timing.Links[master].Contains(slave))
			{
				if (Timing.Links[master].Count <= 1)
				{
					Timing.Links.Remove(master);
				}
				else
				{
					Timing.Links[master].Remove(slave);
				}
				num++;
			}
			if (twoWay && Timing.Links.ContainsKey(slave) && Timing.Links[slave].Contains(master))
			{
				if (Timing.Links[slave].Count <= 1)
				{
					Timing.Links.Remove(slave);
				}
				else
				{
					Timing.Links[slave].Remove(master);
				}
				num++;
			}
			return num;
		}

		[Obsolete("Use Timing.CurrentCoroutine instead.", false)]
		public static float GetMyHandle(Action<CoroutineHandle> reciever)
		{
			Timing._tmpRef = reciever;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.GetHandleHelper);
			return float.NaN;
		}

		private static IEnumerator<float> GetHandleHelper(IEnumerator<float> input, CoroutineHandle handle)
		{
			Action<CoroutineHandle> action = Timing._tmpRef as Action<CoroutineHandle>;
			if (action != null)
			{
				action(handle);
			}
			return input;
		}

		public static float SwitchCoroutine(Segment newSegment)
		{
			Timing._tmpSegment = newSegment;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.SwitchCoroutineRepS);
			return float.NaN;
		}

		private static IEnumerator<float> SwitchCoroutineRepS(IEnumerator<float> coptr, CoroutineHandle handle)
		{
			Timing.GetInstance(handle.Key).RunCoroutineInternal(coptr, Timing._tmpSegment, 0, false, null, handle, false);
			return null;
		}

		public static float SwitchCoroutine(Segment newSegment, string newTag)
		{
			Timing._tmpSegment = newSegment;
			Timing._tmpRef = newTag;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.SwitchCoroutineRepST);
			return float.NaN;
		}

		private static IEnumerator<float> SwitchCoroutineRepST(IEnumerator<float> coptr, CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			instance.RemoveTagOnInstance(handle);
			if (Timing._tmpRef is string)
			{
				instance.AddTagOnInstance((string)Timing._tmpRef, handle);
			}
			instance.RunCoroutineInternal(coptr, Timing._tmpSegment, 0, false, null, handle, false);
			return null;
		}

		public static float SwitchCoroutine(Segment newSegment, int newLayer)
		{
			Timing._tmpSegment = newSegment;
			Timing._tmpInt = newLayer;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.SwitchCoroutineRepSL);
			return float.NaN;
		}

		private static IEnumerator<float> SwitchCoroutineRepSL(IEnumerator<float> coptr, CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			Timing.RemoveLayer(handle);
			instance.AddLayerOnInstance(Timing._tmpInt, handle);
			instance.RunCoroutineInternal(coptr, Timing._tmpSegment, Timing._tmpInt, false, null, handle, false);
			return null;
		}

		public static float SwitchCoroutine(Segment newSegment, int newLayer, string newTag)
		{
			Timing._tmpSegment = newSegment;
			Timing._tmpInt = newLayer;
			Timing._tmpRef = newTag;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.SwitchCoroutineRepSLT);
			return float.NaN;
		}

		private static IEnumerator<float> SwitchCoroutineRepSLT(IEnumerator<float> coptr, CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			instance.RemoveTagOnInstance(handle);
			if (Timing._tmpRef is string)
			{
				instance.AddTagOnInstance((string)Timing._tmpRef, handle);
			}
			Timing.RemoveLayer(handle);
			instance.AddLayerOnInstance(Timing._tmpInt, handle);
			instance.RunCoroutineInternal(coptr, Timing._tmpSegment, Timing._tmpInt, false, null, handle, false);
			return null;
		}

		public static float SwitchCoroutine(string newTag)
		{
			Timing._tmpRef = newTag;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.SwitchCoroutineRepT);
			return float.NaN;
		}

		private static IEnumerator<float> SwitchCoroutineRepT(IEnumerator<float> coptr, CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			instance.RemoveTagOnInstance(handle);
			if (Timing._tmpRef is string)
			{
				instance.AddTagOnInstance((string)Timing._tmpRef, handle);
			}
			return coptr;
		}

		public static float SwitchCoroutine(int newLayer)
		{
			Timing._tmpInt = newLayer;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.SwitchCoroutineRepL);
			return float.NaN;
		}

		private static IEnumerator<float> SwitchCoroutineRepL(IEnumerator<float> coptr, CoroutineHandle handle)
		{
			Timing.RemoveLayer(handle);
			Timing.GetInstance(handle.Key).AddLayerOnInstance(Timing._tmpInt, handle);
			return coptr;
		}

		public static float SwitchCoroutine(int newLayer, string newTag)
		{
			Timing._tmpInt = newLayer;
			Timing._tmpRef = newTag;
			Timing.ReplacementFunction = new Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>>(Timing.SwitchCoroutineRepLT);
			return float.NaN;
		}

		private static IEnumerator<float> SwitchCoroutineRepLT(IEnumerator<float> coptr, CoroutineHandle handle)
		{
			Timing instance = Timing.GetInstance(handle.Key);
			instance.RemoveLayerOnInstance(handle);
			instance.AddLayerOnInstance(Timing._tmpInt, handle);
			instance.RemoveTagOnInstance(handle);
			if (Timing._tmpRef is string)
			{
				instance.AddTagOnInstance((string)Timing._tmpRef, handle);
			}
			return coptr;
		}

		public static CoroutineHandle CallDelayed(float delay, Action action)
		{
			if (action != null)
			{
				return Timing.RunCoroutine(Timing.Instance._DelayedCall(delay, action, null));
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle CallDelayedOnInstance(float delay, Action action)
		{
			if (action != null)
			{
				return this.RunCoroutineOnInstance(this._DelayedCall(delay, action, null));
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle CallDelayed(float delay, Action action, GameObject gameObject)
		{
			if (action != null)
			{
				return Timing.RunCoroutine(Timing.Instance._DelayedCall(delay, action, gameObject), gameObject);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle CallDelayedOnInstance(float delay, Action action, GameObject gameObject)
		{
			if (action != null)
			{
				return this.RunCoroutineOnInstance(this._DelayedCall(delay, action, gameObject), gameObject);
			}
			return default(CoroutineHandle);
		}

		private IEnumerator<float> _DelayedCall(float delay, Action action, GameObject cancelWith)
		{
			yield return this.WaitForSecondsOnInstance(delay);
			if (cancelWith == null || cancelWith != null)
			{
				action();
			}
			yield break;
		}

		public static CoroutineHandle CallDelayed(float delay, Segment segment, Action action)
		{
			if (action != null)
			{
				return Timing.RunCoroutine(Timing.Instance._DelayedCall(delay, action, null), segment);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle CallDelayedOnInstance(float delay, Segment segment, Action action)
		{
			if (action != null)
			{
				return this.RunCoroutineOnInstance(this._DelayedCall(delay, action, null), segment);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle CallDelayed(float delay, Segment segment, Action action, GameObject gameObject)
		{
			if (action != null)
			{
				return Timing.RunCoroutine(Timing.Instance._DelayedCall(delay, action, gameObject), segment, gameObject);
			}
			return default(CoroutineHandle);
		}

		public CoroutineHandle CallDelayedOnInstance(float delay, Segment segment, Action action, GameObject gameObject)
		{
			if (action != null)
			{
				return this.RunCoroutineOnInstance(this._DelayedCall(delay, action, gameObject), segment, gameObject);
			}
			return default(CoroutineHandle);
		}

		public static CoroutineHandle CallPeriodically(float timeframe, float period, Action action, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously(period, action, null));
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.RunCoroutine(Timing.Instance._WatchCall(timeframe, coroutineHandle, null, onDone));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallPeriodicallyOnInstance(float timeframe, float period, Action action, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously(period, action, null));
			if (!float.IsPositiveInfinity(timeframe))
			{
				this.RunCoroutineOnInstance(this._WatchCall(timeframe, coroutineHandle, null, onDone));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallPeriodically(float timeframe, float period, Action action, GameObject gameObject, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously(period, action, gameObject), gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall(timeframe, coroutineHandle, gameObject, onDone), gameObject));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallPeriodicallyOnInstance(float timeframe, float period, Action action, GameObject gameObject, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously(period, action, gameObject), gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall(timeframe, coroutineHandle, gameObject, onDone), gameObject));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallPeriodically(float timeframe, float period, Action action, Segment timing, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously(period, action, null), timing);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.RunCoroutine(Timing.Instance._WatchCall(timeframe, coroutineHandle, null, onDone), timing);
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallPeriodicallyOnInstance(float timeframe, float period, Action action, Segment timing, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously(period, action, null), timing);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall(timeframe, coroutineHandle, null, onDone), timing));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallPeriodically(float timeframe, float period, Action action, Segment timing, GameObject gameObject, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously(period, action, gameObject), timing, gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall(timeframe, coroutineHandle, gameObject, onDone), timing, gameObject));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallPeriodicallyOnInstance(float timeframe, float period, Action action, Segment timing, GameObject gameObject, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously(period, action, gameObject), timing, gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall(timeframe, coroutineHandle, gameObject, onDone), timing, gameObject));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallContinuously(float timeframe, Action action, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously(0f, action, null));
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall(timeframe, coroutineHandle, null, onDone)));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallContinuouslyOnInstance(float timeframe, Action action, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously(0f, action, null));
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall(timeframe, coroutineHandle, null, onDone)));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallContinuously(float timeframe, Action action, GameObject gameObject, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously(0f, action, gameObject), gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall(timeframe, coroutineHandle, gameObject, onDone), gameObject));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallContinuouslyOnInstance(float timeframe, Action action, GameObject gameObject, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously(0f, action, gameObject), gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall(timeframe, coroutineHandle, gameObject, onDone), gameObject));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallContinuously(float timeframe, Action action, Segment timing, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously(0f, action, null), timing);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall(timeframe, coroutineHandle, null, onDone), timing));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallContinuouslyOnInstance(float timeframe, Action action, Segment timing, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously(0f, action, null), timing);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall(timeframe, coroutineHandle, null, onDone), timing));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallContinuously(float timeframe, Action action, Segment timing, GameObject gameObject, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously(0f, action, gameObject), timing, gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall(timeframe, coroutineHandle, gameObject, onDone), timing, gameObject));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallContinuouslyOnInstance(float timeframe, Action action, Segment timing, GameObject gameObject, Action onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously(0f, action, gameObject), timing, gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall(timeframe, coroutineHandle, gameObject, onDone), timing, gameObject));
			}
			return coroutineHandle;
		}

		private IEnumerator<float> _WatchCall(float timeframe, CoroutineHandle handle, GameObject gObject, Action onDone)
		{
			yield return this.WaitForSecondsOnInstance(timeframe);
			this.KillCoroutinesOnInstance(handle);
			if (onDone != null && (gObject == null || gObject != null))
			{
				onDone();
			}
			yield break;
		}

		private IEnumerator<float> _CallContinuously(float period, Action action, GameObject gObject)
		{
			while (gObject == null || gObject != null)
			{
				yield return this.WaitForSecondsOnInstance(period);
				if (gObject == null || (gObject != null && gObject.activeInHierarchy))
				{
					action();
				}
			}
			yield break;
		}

		public static CoroutineHandle CallPeriodically<T>(T reference, float timeframe, float period, Action<T> action, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously<T>(reference, period, action, null));
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall<T>(reference, timeframe, coroutineHandle, null, onDone)));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallPeriodicallyOnInstance<T>(T reference, float timeframe, float period, Action<T> action, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously<T>(reference, period, action, null));
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall<T>(reference, timeframe, coroutineHandle, null, onDone)));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallPeriodically<T>(T reference, float timeframe, float period, Action<T> action, GameObject gameObject, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously<T>(reference, period, action, gameObject), gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall<T>(reference, timeframe, coroutineHandle, gameObject, onDone), gameObject));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallPeriodicallyOnInstance<T>(T reference, float timeframe, float period, Action<T> action, GameObject gameObject, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously<T>(reference, period, action, gameObject), gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall<T>(reference, timeframe, coroutineHandle, gameObject, onDone), gameObject));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallPeriodically<T>(T reference, float timeframe, float period, Action<T> action, Segment timing, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously<T>(reference, period, action, null), timing);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall<T>(reference, timeframe, coroutineHandle, null, onDone), timing));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallPeriodicallyOnInstance<T>(T reference, float timeframe, float period, Action<T> action, Segment timing, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously<T>(reference, period, action, null), timing);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall<T>(reference, timeframe, coroutineHandle, null, onDone), timing));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallPeriodically<T>(T reference, float timeframe, float period, Action<T> action, Segment timing, GameObject gameObject, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously<T>(reference, period, action, gameObject), timing, gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall<T>(reference, timeframe, coroutineHandle, gameObject, onDone), timing, gameObject));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallPeriodicallyOnInstance<T>(T reference, float timeframe, float period, Action<T> action, Segment timing, GameObject gameObject, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously<T>(reference, period, action, gameObject), timing, gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall<T>(reference, timeframe, coroutineHandle, gameObject, onDone), timing, gameObject));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallContinuously<T>(T reference, float timeframe, Action<T> action, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously<T>(reference, 0f, action, null));
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall<T>(reference, timeframe, coroutineHandle, null, onDone)));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallContinuouslyOnInstance<T>(T reference, float timeframe, Action<T> action, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously<T>(reference, 0f, action, null));
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall<T>(reference, timeframe, coroutineHandle, null, onDone)));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallContinuously<T>(T reference, float timeframe, Action<T> action, GameObject gameObject, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously<T>(reference, 0f, action, gameObject), gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall<T>(reference, timeframe, coroutineHandle, gameObject, onDone), gameObject));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallContinuouslyOnInstance<T>(T reference, float timeframe, Action<T> action, GameObject gameObject, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously<T>(reference, 0f, action, gameObject), gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall<T>(reference, timeframe, coroutineHandle, gameObject, onDone), gameObject));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallContinuously<T>(T reference, float timeframe, Action<T> action, Segment timing, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously<T>(reference, 0f, action, null), timing);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall<T>(reference, timeframe, coroutineHandle, null, onDone), timing));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallContinuouslyOnInstance<T>(T reference, float timeframe, Action<T> action, Segment timing, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously<T>(reference, 0f, action, null), timing);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall<T>(reference, timeframe, coroutineHandle, null, onDone), timing));
			}
			return coroutineHandle;
		}

		public static CoroutineHandle CallContinuously<T>(T reference, float timeframe, Action<T> action, Segment timing, GameObject gameObject, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : Timing.RunCoroutine(Timing.Instance._CallContinuously<T>(reference, 0f, action, gameObject), timing, gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, Timing.RunCoroutine(Timing.Instance._WatchCall<T>(reference, timeframe, coroutineHandle, gameObject, onDone), timing, gameObject));
			}
			return coroutineHandle;
		}

		public CoroutineHandle CallContinuouslyOnInstance<T>(T reference, float timeframe, Action<T> action, Segment timing, GameObject gameObject, Action<T> onDone = null)
		{
			CoroutineHandle coroutineHandle = (action == null) ? default(CoroutineHandle) : this.RunCoroutineOnInstance(this._CallContinuously<T>(reference, 0f, action, gameObject), timing, gameObject);
			if (!float.IsPositiveInfinity(timeframe))
			{
				Timing.LinkCoroutines(coroutineHandle, this.RunCoroutineOnInstance(this._WatchCall<T>(reference, timeframe, coroutineHandle, gameObject, onDone), timing, gameObject));
			}
			return coroutineHandle;
		}

		private IEnumerator<float> _WatchCall<T>(T reference, float timeframe, CoroutineHandle handle, GameObject gObject, Action<T> onDone)
		{
			yield return this.WaitForSecondsOnInstance(timeframe);
			this.KillCoroutinesOnInstance(handle);
			if (onDone != null && (gObject == null || gObject != null))
			{
				onDone(reference);
			}
			yield break;
		}

		private IEnumerator<float> _CallContinuously<T>(T reference, float period, Action<T> action, GameObject gObject)
		{
			while (gObject == null || gObject != null)
			{
				yield return this.WaitForSecondsOnInstance(period);
				if (gObject == null || (gObject != null && gObject.activeInHierarchy))
				{
					action(reference);
				}
			}
			yield break;
		}

		[Obsolete("Unity coroutine function, use RunCoroutine instead.", true)]
		public new Coroutine StartCoroutine(IEnumerator routine)
		{
			return null;
		}

		[Obsolete("Unity coroutine function, use RunCoroutine instead.", true)]
		public new Coroutine StartCoroutine(string methodName, object value)
		{
			return null;
		}

		[Obsolete("Unity coroutine function, use RunCoroutine instead.", true)]
		public new Coroutine StartCoroutine(string methodName)
		{
			return null;
		}

		[Obsolete("Unity coroutine function, use RunCoroutine instead.", true)]
		public new Coroutine StartCoroutine_Auto(IEnumerator routine)
		{
			return null;
		}

		[Obsolete("Unity coroutine function, use KillCoroutines instead.", true)]
		public new void StopCoroutine(string methodName)
		{
		}

		[Obsolete("Unity coroutine function, use KillCoroutines instead.", true)]
		public new void StopCoroutine(IEnumerator routine)
		{
		}

		[Obsolete("Unity coroutine function, use KillCoroutines instead.", true)]
		public new void StopCoroutine(Coroutine routine)
		{
		}

		[Obsolete("Unity coroutine function, use KillCoroutines instead.", true)]
		public new void StopAllCoroutines()
		{
		}

		[Obsolete("Use your own GameObject for this.", true)]
		public new static void Destroy(UnityEngine.Object obj)
		{
		}

		[Obsolete("Use your own GameObject for this.", true)]
		public new static void Destroy(UnityEngine.Object obj, float f)
		{
		}

		[Obsolete("Use your own GameObject for this.", true)]
		public new static void DestroyObject(UnityEngine.Object obj)
		{
		}

		[Obsolete("Use your own GameObject for this.", true)]
		public new static void DestroyObject(UnityEngine.Object obj, float f)
		{
		}

		[Obsolete("Use your own GameObject for this.", true)]
		public new static void DestroyImmediate(UnityEngine.Object obj)
		{
		}

		[Obsolete("Use your own GameObject for this.", true)]
		public new static void DestroyImmediate(UnityEngine.Object obj, bool b)
		{
		}

		[Obsolete("Use your own GameObject for this.", true)]
		public new static void Instantiate(UnityEngine.Object obj)
		{
		}

		[Obsolete("Use your own GameObject for this.", true)]
		public new static void Instantiate(UnityEngine.Object original, Vector3 position, Quaternion rotation)
		{
		}

		[Obsolete("Use your own GameObject for this.", true)]
		public new static void Instantiate<T>(T original) where T : UnityEngine.Object
		{
		}

		[Obsolete("Just.. no.", true)]
		public new static T FindObjectOfType<T>() where T : UnityEngine.Object
		{
			return default(T);
		}

		[Obsolete("Just.. no.", true)]
		public new static UnityEngine.Object FindObjectOfType(Type t)
		{
			return null;
		}

		[Obsolete("Just.. no.", true)]
		public new static T[] FindObjectsOfType<T>() where T : UnityEngine.Object
		{
			return null;
		}

		[Obsolete("Just.. no.", true)]
		public new static UnityEngine.Object[] FindObjectsOfType(Type t)
		{
			return null;
		}

		[Obsolete("Just.. no.", true)]
		public new static void print(object message)
		{
		}

		[Tooltip("How quickly the SlowUpdate segment ticks.")]
		public float TimeBetweenSlowUpdateCalls = 0.142857149f;

		[Tooltip("How much data should be sent to the profiler window when it's open.")]
		public DebugInfoType ProfilerDebugAmount;

		[Tooltip("When using manual timeframe, should it run automatically after the update loop or only when TriggerManualTimframeUpdate is called.")]
		public bool AutoTriggerManualTimeframe = true;

		[Tooltip("A count of the number of Update coroutines that are currently running.")]
		[Space(12f)]
		public int UpdateCoroutines;

		[Tooltip("A count of the number of FixedUpdate coroutines that are currently running.")]
		public int FixedUpdateCoroutines;

		[Tooltip("A count of the number of LateUpdate coroutines that are currently running.")]
		public int LateUpdateCoroutines;

		[Tooltip("A count of the number of SlowUpdate coroutines that are currently running.")]
		public int SlowUpdateCoroutines;

		[Tooltip("A count of the number of RealtimeUpdate coroutines that are currently running.")]
		public int RealtimeUpdateCoroutines;

		[Tooltip("A count of the number of EditorUpdate coroutines that are currently running.")]
		public int EditorUpdateCoroutines;

		[Tooltip("A count of the number of EditorSlowUpdate coroutines that are currently running.")]
		public int EditorSlowUpdateCoroutines;

		[Tooltip("A count of the number of EndOfFrame coroutines that are currently running.")]
		public int EndOfFrameCoroutines;

		[Tooltip("A count of the number of ManualTimeframe coroutines that are currently running.")]
		public int ManualTimeframeCoroutines;

		[NonSerialized]
		public float localTime;

		[NonSerialized]
		public float deltaTime;

		public Func<float, float> SetManualTimeframeTime;

		public static Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>> ReplacementFunction;

		public const float WaitForOneFrame = float.NegativeInfinity;

		private static object _tmpRef;

		private static int _tmpInt;

		private static Segment _tmpSegment;

		private int _currentUpdateFrame;

		private int _currentLateUpdateFrame;

		private int _currentSlowUpdateFrame;

		private int _currentRealtimeUpdateFrame;

		private int _currentEndOfFrameFrame;

		private int _nextUpdateProcessSlot;

		private int _nextLateUpdateProcessSlot;

		private int _nextFixedUpdateProcessSlot;

		private int _nextSlowUpdateProcessSlot;

		private int _nextRealtimeUpdateProcessSlot;

		private int _nextEditorUpdateProcessSlot;

		private int _nextEditorSlowUpdateProcessSlot;

		private int _nextEndOfFrameProcessSlot;

		private int _nextManualTimeframeProcessSlot;

		private int _lastUpdateProcessSlot;

		private int _lastLateUpdateProcessSlot;

		private int _lastFixedUpdateProcessSlot;

		private int _lastSlowUpdateProcessSlot;

		private int _lastRealtimeUpdateProcessSlot;

		private int _lastEndOfFrameProcessSlot;

		private int _lastManualTimeframeProcessSlot;

		private float _lastUpdateTime;

		private float _lastLateUpdateTime;

		private float _lastFixedUpdateTime;

		private float _lastSlowUpdateTime;

		private float _lastRealtimeUpdateTime;

		private float _lastEndOfFrameTime;

		private float _lastManualTimeframeTime;

		private float _lastSlowUpdateDeltaTime;

		private float _lastManualTimeframeDeltaTime;

		private ushort _framesSinceUpdate;

		private ushort _expansions = 1;

		[SerializeField]
		[HideInInspector]
		private byte _instanceID;

		private bool _EOFPumpRan;

		private static readonly Dictionary<CoroutineHandle, HashSet<CoroutineHandle>> Links = new Dictionary<CoroutineHandle, HashSet<CoroutineHandle>>();

		private static readonly WaitForEndOfFrame EofWaitObject = new WaitForEndOfFrame();

		private readonly Dictionary<CoroutineHandle, HashSet<CoroutineHandle>> _waitingTriggers = new Dictionary<CoroutineHandle, HashSet<CoroutineHandle>>();

		private readonly HashSet<CoroutineHandle> _allWaiting = new HashSet<CoroutineHandle>();

		private readonly Dictionary<CoroutineHandle, Timing.ProcessIndex> _handleToIndex = new Dictionary<CoroutineHandle, Timing.ProcessIndex>();

		private readonly Dictionary<Timing.ProcessIndex, CoroutineHandle> _indexToHandle = new Dictionary<Timing.ProcessIndex, CoroutineHandle>();

		private readonly Dictionary<CoroutineHandle, string> _processTags = new Dictionary<CoroutineHandle, string>();

		private readonly Dictionary<string, HashSet<CoroutineHandle>> _taggedProcesses = new Dictionary<string, HashSet<CoroutineHandle>>();

		private readonly Dictionary<CoroutineHandle, int> _processLayers = new Dictionary<CoroutineHandle, int>();

		private readonly Dictionary<int, HashSet<CoroutineHandle>> _layeredProcesses = new Dictionary<int, HashSet<CoroutineHandle>>();

		private IEnumerator<float>[] UpdateProcesses = new IEnumerator<float>[256];

		private IEnumerator<float>[] LateUpdateProcesses = new IEnumerator<float>[8];

		private IEnumerator<float>[] FixedUpdateProcesses = new IEnumerator<float>[64];

		private IEnumerator<float>[] SlowUpdateProcesses = new IEnumerator<float>[64];

		private IEnumerator<float>[] RealtimeUpdateProcesses = new IEnumerator<float>[8];

		private IEnumerator<float>[] EditorUpdateProcesses = new IEnumerator<float>[8];

		private IEnumerator<float>[] EditorSlowUpdateProcesses = new IEnumerator<float>[8];

		private IEnumerator<float>[] EndOfFrameProcesses = new IEnumerator<float>[8];

		private IEnumerator<float>[] ManualTimeframeProcesses = new IEnumerator<float>[8];

		private bool[] UpdatePaused = new bool[256];

		private bool[] LateUpdatePaused = new bool[8];

		private bool[] FixedUpdatePaused = new bool[64];

		private bool[] SlowUpdatePaused = new bool[64];

		private bool[] RealtimeUpdatePaused = new bool[8];

		private bool[] EditorUpdatePaused = new bool[8];

		private bool[] EditorSlowUpdatePaused = new bool[8];

		private bool[] EndOfFramePaused = new bool[8];

		private bool[] ManualTimeframePaused = new bool[8];

		private bool[] UpdateHeld = new bool[256];

		private bool[] LateUpdateHeld = new bool[8];

		private bool[] FixedUpdateHeld = new bool[64];

		private bool[] SlowUpdateHeld = new bool[64];

		private bool[] RealtimeUpdateHeld = new bool[8];

		private bool[] EditorUpdateHeld = new bool[8];

		private bool[] EditorSlowUpdateHeld = new bool[8];

		private bool[] EndOfFrameHeld = new bool[8];

		private bool[] ManualTimeframeHeld = new bool[8];

		private CoroutineHandle _eofWatcherHandle;

		private const ushort FramesUntilMaintenance = 64;

		private const int ProcessArrayChunkSize = 64;

		private const int InitialBufferSizeLarge = 256;

		private const int InitialBufferSizeMedium = 64;

		private const int InitialBufferSizeSmall = 8;

		private static Timing[] ActiveInstances = new Timing[16];

		private static Timing _instance;

		private struct ProcessIndex : IEquatable<Timing.ProcessIndex>
		{
			public bool Equals(Timing.ProcessIndex other)
			{
				return this.seg == other.seg && this.i == other.i;
			}

			public override bool Equals(object other)
			{
				return other is Timing.ProcessIndex && this.Equals((Timing.ProcessIndex)other);
			}

			public static bool operator ==(Timing.ProcessIndex a, Timing.ProcessIndex b)
			{
				return a.seg == b.seg && a.i == b.i;
			}

			public static bool operator !=(Timing.ProcessIndex a, Timing.ProcessIndex b)
			{
				return a.seg != b.seg || a.i != b.i;
			}

			public override int GetHashCode()
			{
				return (this.seg - Segment.RealtimeUpdate) * 306783378 + this.i;
			}

			public Segment seg;

			public int i;
		}
	}
}