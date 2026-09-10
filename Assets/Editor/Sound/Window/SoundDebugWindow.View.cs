using System.Collections.Generic;
using System.Linq;
using Sabanishi.MebuMekaFarm.Sound;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Editor.Sound
{
    public partial class SoundDebugWindow
    {
        protected class SoundDebugPanel : PanelBase
        {
            public override string Label => "SoundDebugPanel";

            public SoundDebugPanel(SoundDebugDataStorage storage,
                List<(string name, ISoundDatabase database)> databaseTuples)
            {
                _storage = storage;
                _databaseTuples = databaseTuples;
            }

            private readonly SoundDebugDataStorage _storage;
            private readonly List<(string name, ISoundDatabase database)> _databaseTuples;

            private int _selectCategory = 0;
            private int _selectionViewIndex = 0;
            private Vector2 _scrollArea = Vector2.zero;
            private Vector2 _displayArea = Vector2.zero;

            protected override void DrawGuiInternal(SoundDebugWindow window)
            {
                // カテゴリ選択
                var nameList = new string[] { "BGM", "SE" };
                _selectCategory = GUILayout.SelectionGrid(_selectCategory, nameList, 2, GUILayout.Width(300));

                // 選択カテゴリ内のPlaybackUnitの状態を表示
                using (new GUILayout.VerticalScope(window.DisplayStyle))
                {
                    GUILayout.Space(5);
                    using (var scrollView = new GUILayout.ScrollViewScope(_scrollArea))
                    {
                        _scrollArea = scrollView.scrollPosition;
                        GUILayout.Space(0);

                        foreach (var data in _storage.GetPlaybackUnitDataList(_selectCategory))
                        {
                            using (new GUILayout.HorizontalScope("Box", GUILayout.ExpandWidth(true),
                                       GUILayout.MaxHeight(28)))
                            {
                                GUILayout.Label("", window.DisplayStyle, GUILayout.Width(10));
                                GUILayout.Label(data.name, GUILayout.Width(100));
                                GUILayout.Label(data.scriptName, GUILayout.Width(150));
                                GUILayout.Label(">");
                                GUILayout.Label(data.clipName, GUILayout.Width(150));
                                using (new GUILayout.VerticalScope("Box", GUILayout.Width(20)))
                                {
                                    GUILayout.Box("", window.VolumeStyle, GUILayout.Height(15 * data.volume),
                                        GUILayout.Width(15));
                                }

                                // ループ表示
                                var loopMark = data.loopUse ? "L" : " ";
                                GUILayout.Label(loopMark, window.LoopStyle, GUILayout.Width(15));

                                // Playステータスの表示
                                var playMode = data.playing ? "Playing" : "Stop";
                                var playModeStyle = data.playing ? window.PlayingStyle : window.StopStyle;
                                GUILayout.Label(playMode, playModeStyle, GUILayout.Width(60));

                                // 再生時間(%)の表示
                                const int playLengthWidth = 200;
                                using (new GUILayout.HorizontalScope("Box", GUILayout.Width(playLengthWidth)))
                                {
                                    GUILayout.Box("", window.PlayingTimeStyle,
                                        GUILayout.Width(data.playingTimeRatio * playLengthWidth), GUILayout.Height(10));
                                }
                            }
                        }
                    }
                }

                GUILayout.Space(10);

                using (new GUILayout.HorizontalScope("Box", GUILayout.Width(500)))
                {
                    using (new GUILayout.VerticalScope(GUILayout.Width(500)))
                    {
                        if (GUILayout.Button("変数デバッグ"))
                        {
                            _selectionViewIndex = 0;
                        }

                        if (GUILayout.Button("クリップ参照チェック"))
                        {
                            _selectionViewIndex = 1;
                        }

                        using (var scrollView = new GUILayout.ScrollViewScope(_displayArea, window.DisplayStyle,
                                   GUILayout.Height(300)))
                        {
                            _displayArea = scrollView.scrollPosition;
                            switch (_selectionViewIndex)
                            {
                                case 0:
                                    foreach (var checkColumn in _storage.GetVariableDataList())
                                    {
                                        using (new GUILayout.HorizontalScope("Box"))
                                        {
                                            GUILayout.Label(checkColumn.name, GUILayout.Width(180));
                                            GUILayout.Box("|");
                                            GUILayout.Label(checkColumn.value);
                                        }
                                    }

                                    break;
                                case 1:
                                    // AudioClip参照チェック
                                    var nullCheckList = new List<string>();
                                    foreach (var (name, database) in _databaseTuples)
                                    {
                                        nullCheckList.AddRange(database.LookupReferenceMissingClipNames()
                                            .Select(x => name + ":" + x));
                                    }

                                    if (nullCheckList.Count == 0)
                                    {
                                        nullCheckList.Add("参照が外れているAudioClipはありません");
                                    }
                                    else
                                    {
                                        nullCheckList.Insert(0, "参照が外れているAudioClipは以下の通りです");
                                    }

                                    foreach (var s in nullCheckList)
                                    {
                                        GUILayout.Label(s, GUILayout.Width(300));
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}