using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Editor
{
    /// <summary>
    /// デバッグ用ウィンドウの基底
    /// </summary>
    public abstract class DebugWindowBase<TWindow> : EditorWindow where TWindow : DebugWindowBase<TWindow>
    {
        /// <summary>
        /// パネル基底
        /// </summary>
        public abstract class PanelBase : IDisposable
        {
            private Vector2 _scroll = Vector2.zero;
            private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
            private CancellationTokenSource _cts;
            private readonly Dictionary<string, bool> _foldouts = new Dictionary<string, bool>();
            private readonly Dictionary<string, Vector2> _scrolls = new Dictionary<string, Vector2>();

            /// <summary>表示ラベル</summary>
            public abstract string Label { get; }

            /// <summary>非同期キャンセルチェック用トークン</summary>
            protected CancellationToken Token => _cancellationTokenSource.Token;

            /// <summary>
            /// 開始処理
            /// </summary>
            public void Start(TWindow window)
            {
                if (_cts != null)
                {
                    return;
                }

                _cts = new CancellationTokenSource();
                StartInternal(window, _cts.Token);
            }

            /// <summary>
            /// 終了処理
            /// </summary>
            public void Exit(TWindow window)
            {
                if (_cts == null)
                {
                    return;
                }

                var cts = _cts;
                _cts = null;
                ExitInternal(window);
                cts.Dispose();
            }

            /// <summary>
            /// 廃棄時処理
            /// </summary>
            public void Dispose()
            {
                if (_cts == null)
                {
                    return;
                }

                DisposeInternal();
                _cts.Dispose();
                _cts = null;
            }

            /// <summary>
            /// Gui描画
            /// </summary>
            public void DrawGui(TWindow window)
            {
                using (var scope = new EditorGUILayout.ScrollViewScope(_scroll))
                {
                    DrawGuiInternal(window);
                    _scroll = scope.scrollPosition;
                }

                if (GUILayout.Button("Cancel"))
                {
                    CancelToken();
                }
            }

            /// <summary>
            /// SceneビューのGui描画
            /// </summary>
            public void DrawSceneGui(SceneView sceneView, TWindow window)
            {
                if (window == null)
                {
                    return;
                }

                DrawSceneGuiInternal(sceneView, window);
            }

            /// <summary>
            /// 開始処理
            /// </summary>
            protected virtual void StartInternal(TWindow window, CancellationToken token)
            {
            }

            /// <summary>
            /// 終了処理
            /// </summary>
            protected virtual void ExitInternal(TWindow window)
            {
            }

            /// <summary>
            /// 廃棄処理
            /// </summary>
            protected virtual void DisposeInternal()
            {
            }

            /// <summary>
            /// Gui描画
            /// </summary>
            protected abstract void DrawGuiInternal(TWindow window);

            /// <summary>
            /// SceneビューのGui描画
            /// </summary>
            protected virtual void DrawSceneGuiInternal(SceneView sceneView, TWindow window)
            {
            }

            /// <summary>
            /// 非同期キャンセル用トークンをキャンセル
            /// </summary>
            protected void CancelToken()
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
            }

            /// <summary>
            /// コンテンツの描画
            /// </summary>
            protected void DrawContent(string title, Action onDraw)
            {
                using (new EditorGUILayout.VerticalScope("Box"))
                {
                    EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    onDraw?.Invoke();
                    EditorGUI.indentLevel--;
                }
            }

            /// <summary>
            /// コンテンツの描画
            /// </summary>
            protected void DrawFoldoutContent(string title, Action onDraw)
            {
                if (!_foldouts.TryGetValue(title, out var foldout))
                {
                    _foldouts[title] = foldout;
                }

                _foldouts[title] = EditorGUILayout.Foldout(foldout, title, EditorStyles.foldoutHeader);
                if (_foldouts[title])
                {
                    using (new EditorGUILayout.VerticalScope("Box"))
                    {
                        EditorGUI.indentLevel++;
                        onDraw?.Invoke();
                        EditorGUI.indentLevel--;
                    }
                }
            }

            /// <summary>
            /// コンテンツの描画
            /// </summary>
            protected void DrawScrollContent(string title, float height, Action onDraw)
            {
                if (!_scrolls.TryGetValue(title, out var scroll))
                {
                    _scrolls[title] = Vector2.zero;
                }

                using (var scope = new EditorGUILayout.ScrollViewScope(scroll, "Box", GUILayout.MaxHeight(height)))
                {
                    EditorGUI.indentLevel++;
                    onDraw?.Invoke();
                    EditorGUI.indentLevel--;
                    _scrolls[title] = scope.scrollPosition;
                }
            }
        }

        private readonly List<PanelBase> _panels = new();

        private int _currentTabIndex;

        /// <summary>更新するか</summary>
        protected virtual bool IsActive => UnityEngine.Application.isPlaying;

        /// <summary>パネルを描画するか</summary>
        protected virtual bool IsDrawPanel => true;

        /// <summary>パネルリスト</summary>
        protected IReadOnlyList<PanelBase> Panels => _panels;

        /// <summary>
        /// アクティブ時処理
        /// </summary>
        protected virtual void OnEnableInternal()
        {
        }

        /// <summary>
        /// 非アクティブ時処理
        /// </summary>
        protected virtual void OnDisableInternal()
        {
        }

        /// <summary>
        /// エラーメッセージの表示
        /// </summary>
        protected virtual string GetGuiErrorMessage()
        {
            return null;
        }

        /// <summary>
        /// ヘッダー要素のGUI描画
        /// </summary>
        protected virtual void OnHeaderGuiInternal()
        {
        }

        /// <summary>
        /// フッター要素のGUI描画
        /// </summary>
        protected virtual void OnFooterGuiInternal()
        {
        }

        /// <summary>
        /// パネルの追加
        /// </summary>
        protected void AddPanel(PanelBase panel)
        {
            _panels.Add(panel);
        }

        /// <summary>
        /// アクティブ時処理
        /// </summary>
        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;

            // パネルの作成
            _panels.Clear();
            OnEnableInternal();
        }

        /// <summary>
        /// 非アクティブ時処理
        /// </summary>
        private void OnDisable()
        {
            OnDisableInternal();

            foreach (var panel in _panels)
            {
                panel.Dispose();
            }

            _panels.Clear();
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        /// <summary>
        /// GUI描画
        /// </summary>
        private void OnGUI()
        {
            if (!IsActive)
            {
                foreach (var panel in _panels)
                {
                    panel.Exit((TWindow)this);
                }

                return;
            }

            var errorMessage = GetGuiErrorMessage();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                return;
            }

            foreach (var panel in _panels)
            {
                panel.Start((TWindow)this);
            }

            // ヘッダー
            OnHeaderGuiInternal();

            // カレントなパネルGUIの描画
            var labels = _panels.Select(x => x.Label).ToArray();
            _currentTabIndex = GUILayout.Toolbar(_currentTabIndex, labels, EditorStyles.toolbarButton);

            if (_currentTabIndex >= 0)
            {
                if (IsDrawPanel)
                {
                    _panels[_currentTabIndex].DrawGui((TWindow)this);
                }
            }

            // フッター
            OnFooterGuiInternal();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            Repaint();
        }

        /// <summary>
        /// SceneビューのGUI描画
        /// </summary>
        private void OnSceneGUI(SceneView sceneView)
        {
            if (!IsActive)
            {
                return;
            }

            // カレントなパネルGUIの描画
            if (_currentTabIndex >= 0)
            {
                if (IsDrawPanel)
                {
                    _panels[_currentTabIndex].DrawSceneGui(sceneView, (TWindow)this);
                }
            }
        }
    }
}