/* Copyright(C) 2016  Teambrella, Inc.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License(version 3) as published
 * by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see<http://www.gnu.org/licenses/>.
 */
using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Web;
using Newtonsoft.Json;
using Teambrella.Client.DomainModel;
using Teambrella.Client.ServerApiModels;
using Teambrella.Client.Service;
using Teambrella.Client.Services;

namespace Teambrella.Client.App
{
    public class TeambrellaApp : Form
    {
        private class TxTag
        {
            public readonly Guid Id;

            public TxTag(Guid id)
            {
                Id = id;
            }

            public static bool Equals(TxTag t1, TxTag t2)
            {
                if (ReferenceEquals(t1, t2)) return true;

                if (t1 == null || t2 == null) return false;

                return t1.Id == t2.Id;
            }
        }

        const char NBSP = '\u00a0';
        const string NBSP_offset = "\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0"; // need this to expand anchors

        public AccountService _accountService { get; set; }

        private DateTime _updateTime;
        private int _curRow = -1;
        private int _curColumn = -1;
        private int _curSubRow = -1;

        private int _cellFontHeight;
        private Graphics _graphics;
        private ToolTip _tooltip;
        private string _tooltipText;
        private Font _mainFont;
        private Font _underlinedFont;
        private Font _iconsFont;
        private Font _icons2Font;
        private Padding _cellPadding;

        private NotifyIcon _trayIcon;
        private DataGridView _myTxGridView;
        private DataGridView _cosignTxGridView;
        private ContextMenu _trayMenu;

        const string ColInitiated = "Initiated";
        const string ColAmount = "Amount";
        const string ColFrom = "From";
        const string ColTo = "To";
        const string ColDescription = "Description";
        const string ColStatus = "Status";
        const string ColApprove = "Approve";
        const string ColBlock = "Block";

        [STAThread]
        public static void Main()
        {
            //BlockchainService = new BlockchainService(AccountService);

            TestService();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TeambrellaApp());
       }

        private static TxTag GetFromTx(Tx tx)
        {
            var res = new TxTag(tx.Id);
            return res;
        }

        public TeambrellaApp()
        {
            Logger.WriteMessage("App is started");
            _trayMenu = new ContextMenu()
            {
                MenuItems =
                {
                    new MenuItem("My Account", (s,e) =>
                        {
                            OpenBrowser();
                        }),
                    new MenuItem("Check Transactions", (s,e) =>
                        {
                            _accountService = new AccountService();
                            UpdateTxLists(_accountService);
                            ShowInTaskbar = true;
                            Show();
                            BringToFront();
                        }),
                    new MenuItem("Team Settings", (s,e) =>
                        {
                            ShowSettings();
                        }),
                    new MenuItem("-"),
                    new MenuItem("Exit", (s,e) =>
                        {
                            Application.Exit();
                        }),
                }
            };
            _trayMenu.MenuItems[0].DefaultItem = true;

            _trayIcon = new NotifyIcon()
            {
                Text = "Teambrella",
                Icon = new Icon(SystemIcons.Application, 40, 40),
                Visible = true,
                ContextMenu = _trayMenu,
            };

            InitializeComponent();
            using (var accountService = new AccountService())
            {
                UpdateTxLists(accountService);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (_accountService != null && !_accountService.Closed)
                _accountService.Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }


        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void OpenBrowser()
        {
            OpenBrowser(null, null, null, false);
        }

        private void OpenBrowser(int? teamId, int? claimId, int? teammateId, bool isWithdrawal)
        {
            string modelString;
            using (var accountService = new Services.AccountService())
            {
                var modelIn = accountService.GetLoginQueryObject();
                modelIn.TeamId = teamId;
                modelIn.ClaimId = claimId;
                modelIn.TeammateId = teammateId;
                modelIn.IsWithdrawal = isWithdrawal;
                modelString = JsonConvert.SerializeObject(modelIn);
            }

            System.Diagnostics.Process.Start(Server.GetSiteUrl() + "/me/ClientLogin?data=" + HttpUtility.UrlEncode(modelString));
        }

        static void TestService()
        {
            var winService = new TeambrellaService();
            winService.Run();
        }

        private void ShowSettings()
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }

        private void InitializeComponent()
        {
            _mainFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            _underlinedFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            _iconsFont = new System.Drawing.Font("Webdings", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            _icons2Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));

            DataGridViewCellStyle styleMain = new DataGridViewCellStyle
            {
                WrapMode = DataGridViewTriState.True,
                Alignment = DataGridViewContentAlignment.TopLeft,
                Padding = new Padding(5, 5, 5, 5),
                SelectionBackColor = System.Drawing.Color.White,
                SelectionForeColor = System.Drawing.Color.Black,
                Font = _mainFont,
            };

            DataGridViewCellStyle styleDate = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.TopLeft,
                Padding = new Padding(5, 5, 5, 5),
                SelectionBackColor = System.Drawing.Color.White,
                SelectionForeColor = System.Drawing.Color.Black,
                Font = _mainFont,
            };


            DataGridViewCellStyle styleButton = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new Padding(5, 5, 5, 5),
                SelectionBackColor = System.Drawing.Color.White,
                SelectionForeColor = System.Drawing.Color.Black,
                Font = _mainFont,
            };

            _cellFontHeight = styleMain.Font.Height;
            _cellPadding = styleMain.Padding;
            _graphics = CreateGraphics();
            _tooltip = new ToolTip
            {
                OwnerDraw = true,
            };
            _tooltip.Draw += (sender, e) =>
                {
                    e.DrawBackground();
                    e.DrawBorder();
                    e.Graphics.DrawString(_tooltipText, _mainFont, Brushes.Black, new PointF(2, 2));
                };
            _tooltip.Popup += (sender, e) =>
               {
                   var size = TextRenderer.MeasureText(_tooltipText, _mainFont);
                   size.Height += 5;
                   size.Width += 3;
                   e.ToolTipSize = size;
               };

            _myTxGridView = new DataGridView
            {
                AllowUserToAddRows = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                BackgroundColor = System.Drawing.SystemColors.Control,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                Location = new System.Drawing.Point(0, 0),
                Name = "MyTxGridView",
                RowHeadersVisible = false,
                ColumnHeadersHeight = 30,
                Size = new System.Drawing.Size(1084, 551),
                TabIndex = 0,
                ShowCellToolTips = false,
                Columns =
                {
                    new DataGridViewTextBoxColumn
                    {
                        DefaultCellStyle = styleDate,
                        HeaderText = "Initiated",
                        Name = ColInitiated,
                        ToolTipText = "Date and time when the transaction got recieved by your client software",
                    },
                    new DataGridViewLinkColumn
                    {
                        DefaultCellStyle = styleMain,
                        HeaderText = "Transaction",
                        Name = ColDescription,
                    },
                    new DataGridViewLinkColumn
                    {
                        DefaultCellStyle = styleMain,
                        HeaderText = "To",
                        Name = ColTo,
                    },
                    new DataGridViewTextBoxColumn
                    {
                        DefaultCellStyle = styleMain,
                        HeaderText = "Amount",
                        Name = ColAmount,
                    },
                    new DataGridViewTextBoxColumn
                    {
                        DefaultCellStyle = styleDate,
                        HeaderText = "Status",
                        Name = ColStatus,
                    },
                    new DataGridViewButtonColumn
                    {
                        DefaultCellStyle = styleButton,
                        HeaderText = "",
                        Name = ColApprove,
                    },
                    new DataGridViewButtonColumn
                    {
                        DefaultCellStyle = styleButton,
                        HeaderText = "",
                        Name = ColBlock,
                    },
                }
            };
            _myTxGridView.CellContentClick += new DataGridViewCellEventHandler(CellContentClick);
            _myTxGridView.CellPainting += new DataGridViewCellPaintingEventHandler(CellPainting);
            _myTxGridView.CellMouseLeave += new DataGridViewCellEventHandler(CellMouseLeave);
            _myTxGridView.CellMouseEnter += new DataGridViewCellEventHandler(CellMouseEnter);
            _myTxGridView.CellMouseMove += new DataGridViewCellMouseEventHandler(CellMouseMove);

            _cosignTxGridView = new DataGridView
            {
                AllowUserToAddRows = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                BackgroundColor = System.Drawing.SystemColors.Control,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                Location = new System.Drawing.Point(0, 0),
                Name = "MyTxGridView",
                RowHeadersVisible = false,
                ColumnHeadersHeight = 30,
                Size = new System.Drawing.Size(1084, 551),
                TabIndex = 0,
                ShowCellToolTips = false,
                Columns =
                {
                    new DataGridViewTextBoxColumn
                    {
                        DefaultCellStyle = styleDate,
                        HeaderText = "Initiated",
                        Name = ColInitiated,
                        ToolTipText = "Date and time when the transaction got recieved by your client software",
                    },
                    new DataGridViewLinkColumn
                    {
                        DefaultCellStyle = styleMain,
                        HeaderText = "From",
                        Name = ColFrom,
                    },
                    new DataGridViewLinkColumn
                    {
                        DefaultCellStyle = styleMain,
                        HeaderText = "Transaction",
                        Name = ColDescription,
                    },
                    new DataGridViewLinkColumn
                    {
                        DefaultCellStyle = styleMain,
                        HeaderText = "To",
                        Name = ColTo,
                    },
                    new DataGridViewTextBoxColumn
                    {
                        DefaultCellStyle = styleMain,
                        HeaderText = "Amount",
                        Name = ColAmount,
                    },
                    new DataGridViewTextBoxColumn
                    {
                        DefaultCellStyle = styleDate,
                        HeaderText = "Status",
                        Name = ColStatus,
                    },
                    new DataGridViewButtonColumn
                    {
                        DefaultCellStyle = styleButton,
                        HeaderText = "",
                        Name = ColApprove,
                    },
                    new DataGridViewButtonColumn
                    {
                        DefaultCellStyle = styleButton,
                        HeaderText = "",
                        Name = ColBlock,
                    },
                }
            };
            _cosignTxGridView.CellContentClick += new DataGridViewCellEventHandler(CellContentClick);
            _cosignTxGridView.CellPainting += new DataGridViewCellPaintingEventHandler(CellPainting);
            _cosignTxGridView.CellMouseLeave += new DataGridViewCellEventHandler(CellMouseLeave);
            _cosignTxGridView.CellMouseEnter += new DataGridViewCellEventHandler(CellMouseEnter);
            _cosignTxGridView.CellMouseMove += new DataGridViewCellMouseEventHandler(CellMouseMove);


            TabControl tabControl = new TabControl
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Font = _mainFont,
                Location = new System.Drawing.Point(0, 5),
                Name = "_tabControl",
                SelectedIndex = 0,
                Size = new System.Drawing.Size(1092, 579),
                TabIndex = 1,
                Padding = new Point(14, 6),
                Controls =
                {
                    new TabPage
                    {
                        Location = new System.Drawing.Point(2, 24),
                        Name = "_tabFromMe",
                        Padding = new Padding(0),
                        Size = new System.Drawing.Size(1084, 551),
                        TabIndex = 0,
                        Text = "From Me",
                        UseVisualStyleBackColor = true,
                        Controls =
                        {
                            _myTxGridView
                        }
                    },
                    new TabPage
                    {
                        Location = new System.Drawing.Point(2, 24),
                        Name = "_tabCosigned",
                        Padding = new Padding(0),
                        Size = new System.Drawing.Size(1084, 551),
                        TabIndex = 1,
                        Text = "With My Cosignature",
                        UseVisualStyleBackColor = true,
                        Controls =
                        {
                            _cosignTxGridView
                        }
                    },
                }
            };

            ClientSize = new System.Drawing.Size(1091, 583);
            Controls.Add(tabControl);

            _myTxGridView.Columns[ColAmount].Width = 100;
            _myTxGridView.Columns[ColTo].Width = 200;
            _myTxGridView.Columns[ColDescription].Width = 230;
            _myTxGridView.Columns[ColInitiated].Width = 120;
            _myTxGridView.Columns[ColStatus].Width = 250;
            _myTxGridView.Columns[ColApprove].Width = 80;
            _myTxGridView.Columns[ColBlock].Width = 80;

            _cosignTxGridView.Columns[ColFrom].Width = 160;
            _cosignTxGridView.Columns[ColAmount].Width = 100;
            _cosignTxGridView.Columns[ColTo].Width = 170;
            _cosignTxGridView.Columns[ColDescription].Width = 170;
            _cosignTxGridView.Columns[ColInitiated].Width = 120;
            _cosignTxGridView.Columns[ColStatus].Width = 200;
            _cosignTxGridView.Columns[ColApprove].Width = 80;
            _cosignTxGridView.Columns[ColBlock].Width = 80;

            Name = "Teambrella";
            Text = "Teambrella - Transactions";

            FormClosing += (s, e) =>
                {
                    if (e.CloseReason != CloseReason.UserClosing)
                    {
                        return;
                    }
                    this.Hide();
                    e.Cancel = true;
                };
        }

        private void CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var gridView = (DataGridView)sender;
            var row = gridView.Rows[e.RowIndex];
            var cell = row.Cells[e.ColumnIndex];
            var cellName = cell.OwningColumn.Name;

            TxTag txTag = cell.Tag as TxTag;

            if (txTag == null)
            {
                return;
            }
            Tx tx = _accountService.GetTx(txTag.Id);

            if (!tx.Teammate.Team.IsInNormalState)
            {
                return;
            }

            if (cellName == ColFrom)
            {
                OpenBrowser(tx.Teammate.TeamId, null, tx.Teammate.Id, false);
            }
            if (cellName == ColTo)
            {
                var output = tx.Outputs.OrderByDescending(x => x.AmountBTC).ToList()[_curSubRow];
                OpenBrowser(tx.Teammate.TeamId, null, output.PayTo.Teammate.Id, false);
            }
            if (cellName == ColDescription && tx.Kind == TxKind.Payout)
            {
                OpenBrowser(tx.Teammate.TeamId, tx.ClaimId.Value, null, false);
            }
            if (cellName == ColDescription && tx.Kind == TxKind.Withdraw)
            {
                OpenBrowser(tx.Teammate.TeamId, null, null, true);
            }
            if (cellName == ColApprove && cell is DataGridViewButtonCell)
            {
                _accountService.ChangeTxResolution(tx, TxClientResolution.Approved);
                UpdateTxStatus(_accountService, row);
                _accountService.SaveChanges();

            }
            if (cellName == ColBlock && cell is DataGridViewButtonCell)
            {
                _accountService.ChangeTxResolution(tx, tx.Resolution == TxClientResolution.Blocked ? TxClientResolution.Received : TxClientResolution.Blocked);
                UpdateTxStatus(_accountService, row);
                _accountService.SaveChanges();
            }
        }

        private void CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            var gridView = (DataGridView)sender;
            var cell = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var cellName = cell.OwningColumn.Name;
            if (cellName == ColAmount || cellName == ColTo || cellName == ColDescription || cellName == ColFrom)
            {
                _curSubRow = -1;
                gridView.InvalidateCell(cell);
            }
        }

        private void CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            var gridView = (DataGridView)sender;
            var cell = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var cellName = cell.OwningColumn.Name;
            if (cellName == ColAmount || cellName == ColTo || cellName == ColDescription || cellName == ColFrom)
            {
                _curSubRow = -1;
                _tooltip.SetToolTip(gridView, null);
                gridView.InvalidateCell(cell);
            }
        }

        private void CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            var gridView = (DataGridView)sender;
            _curRow = e.RowIndex;
            _curColumn = e.ColumnIndex;
            if (e.RowIndex == -1)
            {
                return;
            }
            var cell = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var cellName = cell.OwningColumn.Name;
            if (cellName == ColAmount || cellName == ColTo || cellName == ColDescription || cellName == ColFrom)
            {
                var subRow = (e.Location.Y - _cellPadding.Top) / (_cellFontHeight + 1);
                var txTag = (TxTag)cell.Tag;
                var tx = _accountService.GetTx(txTag.Id);

                var text = GetSubRowText(tx, cell, subRow);
                if (text == null)
                {
                    return;
                }
                SizeF textArea = _graphics.MeasureString(text, _underlinedFont);
                int padding = cell.OwningColumn.DefaultCellStyle.Padding.Left;
                if (e.Location.X < padding || e.Location.X > textArea.Width + padding)
                {
                    subRow = -1;
                }
                if (subRow != _curSubRow)
                {
                    _curSubRow = subRow;
                    if (subRow >= 0 && cellName != ColFrom)
                    {
                        var txOutputs = tx.Outputs.OrderByDescending(x => x.AmountBTC).ToList();
                        if (txOutputs.Count <= subRow)
                        {
                            return;
                        }
                        var txOutput = txOutputs[subRow];
                        if (cellName == ColTo)
                        {
                            int totalDays = (int)(DateTime.UtcNow - txOutput.PayTo.KnownSince).TotalDays;
                            _tooltipText =
                                "Bitcoin Address: " + txOutput.PayTo.Address + Environment.NewLine
                                + "Known For: " + totalDays + ((totalDays == 1) ? " day" : " days");
                            _tooltip.SetToolTip(gridView, _tooltipText);
                            _tooltip.BackColor = _accountService.IsPayToAddressOkAge(txOutput) ? Color.FromArgb(233, 255, 225) : Color.FromArgb(255, 244, 187);

                        }
                    }
                    else
                    {
                        _tooltip.SetToolTip(gridView, null);
                    }
                    gridView.InvalidateCell(cell);
                }
            }
            else
            {
                _tooltip.SetToolTip(gridView, null);
            }
        }

        private string GetSubRowText(Tx tx, DataGridViewCell cell, int subRow)
        {
            //var txTag = (TxTag)cell.Tag;

            //var tx = _accountService.GetTx(txTag.Id);

            var cellName = cell.OwningColumn.Name;
            if (cellName == ColFrom)
            {
                return tx.Teammate.Name.Replace(' ', NBSP);
            }

            var txOutputs = tx.Outputs.OrderByDescending(x => x.AmountBTC).ToList();
            if (txOutputs.Count <= subRow)
            {
                return null;
            }
            var txOutput = txOutputs[subRow];

            if (cellName == ColAmount)
            {
                return (-txOutput.AmountBTC * 1000).ToString("N2") + NBSP + "mBTC";
            }
            if (cellName == ColTo)
            {
                return NBSP_offset + txOutput.PayTo.Teammate.Name.Replace(' ', NBSP);
            }
            if (cellName == ColDescription && tx.Kind == TxKind.Payout)
            {
                return "Claim" + NBSP + tx.ClaimId.ToString();
            }
            if (cellName == ColDescription && tx.Kind == TxKind.Withdraw)
            {
                return "Withdrawal";
            }
            return null;
        }

        private void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            var gridView = (DataGridView)sender;
            var cell = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var cellName = cell.OwningColumn.Name;
            var isSubrowCell = (cellName == ColAmount || cellName == ColTo || cellName == ColDescription);
            var isDateCell = (cellName == ColInitiated);
            var isFromCell = (cellName == ColFrom);
            var isStatusCell = (cellName == ColStatus);

            if (!isSubrowCell && !isDateCell && !isStatusCell && !isFromCell)
            {
                return;
            }

            using (
                Brush gridBrush = new SolidBrush(gridView.GridColor),
                backColorBrush = new SolidBrush(e.CellStyle.BackColor))
            {
                using (Pen gridLinePen = new Pen(gridBrush))
                {
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                }
            }

            var startPoint = new PointF(e.CellBounds.X + cell.OwningColumn.DefaultCellStyle.Padding.Left, e.CellBounds.Y + cell.OwningColumn.DefaultCellStyle.Padding.Top);
            if (e.Value != null && isSubrowCell)
            {
                TxTag txTag = (TxTag)cell.Tag;
                Tx tx = _accountService.GetTx(txTag.Id);

                int subRow = 0;
                foreach (var txOutput in tx.Outputs.OrderByDescending(x => x.AmountBTC))
                {
                    var text = GetSubRowText(tx, cell, subRow);
                    SizeF textArea = _graphics.MeasureString(text, _underlinedFont);
                    var startPoint2 = new PointF { X = startPoint.X + textArea.Width, Y = startPoint.Y };
                    var brush = Brushes.Black;
                    var font = e.CellStyle.Font;
                    if (cellName == ColTo
                        || cellName == ColDescription && tx.Kind == TxKind.Payout
                        || cellName == ColDescription && tx.Kind == TxKind.Withdraw)
                    {
                        brush = Brushes.RoyalBlue;
                        if (_curSubRow == subRow && _curRow == e.RowIndex && _curColumn == e.ColumnIndex)
                        {
                            font = _underlinedFont;
                        }
                    }

                    e.Graphics.DrawString(text, font, brush, startPoint, StringFormat.GenericDefault);

                    bool goodPayToAddress = _accountService.IsPayToAddressOkAge(txOutput);
                    if (cellName == ColTo && !goodPayToAddress)
                    {
                        e.Graphics.DrawString("\xEA", _iconsFont, Brushes.Goldenrod, startPoint.X, startPoint.Y - 2, StringFormat.GenericDefault);
                        e.Graphics.DrawString("!", _icons2Font, Brushes.Goldenrod, startPoint.X + 8F, startPoint.Y + 2, StringFormat.GenericDefault);
                    }
                    if (cellName == ColTo && goodPayToAddress)
                    {
                        e.Graphics.DrawString("\u25CF", _icons2Font, Brushes.MediumSeaGreen, startPoint.X + 6, startPoint.Y + 2, StringFormat.GenericDefault);
                    }

                    if (cellName == ColDescription && tx.Kind == TxKind.Payout)
                    {
                        string reason = " - reimbursement";
                        if (txOutput.PayTo.TeammateId != tx.ClaimTeammateId)
                        {
                            reason = " - for voting";
                        }
                        e.Graphics.DrawString(reason, e.CellStyle.Font, Brushes.Black, startPoint2, StringFormat.GenericDefault);
                    }

                    startPoint.Y += (_cellFontHeight + 1);
                    subRow++;
                }
            }

            if (e.Value != null && isFromCell)
            {
                var font = _mainFont;
                if (_curSubRow == 0 && _curRow == e.RowIndex && _curColumn == e.ColumnIndex)
                {
                    font = _underlinedFont;
                }
                TxTag txTag = (TxTag)cell.Tag;
                Tx tx = _accountService.GetTx(txTag.Id);

                var text = GetSubRowText(tx, cell, 0);
                e.Graphics.DrawString(text, font, Brushes.RoyalBlue, startPoint, StringFormat.GenericDefault);
            }


            if (e.Value != null && isDateCell)
            {
                string textDate = ((DateTime)cell.Tag).ToShortDateString();
                string textTime = ((DateTime)cell.Tag).ToLongTimeString();
                SizeF textArea = _graphics.MeasureString(textDate, _underlinedFont);
                var startPoint2 = new PointF { X = startPoint.X + textArea.Width, Y = startPoint.Y };
                e.Graphics.DrawString(textDate, e.CellStyle.Font, Brushes.Black, startPoint, StringFormat.GenericDefault);
                e.Graphics.DrawString(textTime, e.CellStyle.Font, Brushes.Silver, startPoint2, StringFormat.GenericDefault);
            }

            if (e.Value != null && isStatusCell)
            {
                TxTag txTag = (TxTag)cell.Tag;
                Tx tx = _accountService.GetTx(txTag.Id);
                var statusText = _accountService.GetTxStatusText(tx, gridView == _myTxGridView);
                e.Graphics.DrawString(statusText, e.CellStyle.Font, Brushes.Black, startPoint, StringFormat.GenericDefault);
            }

            e.Handled = true;
        }

        private void UpdateTxStatus(AccountService accountService, DataGridViewRow row)
        {
            TxTag txTag = (TxTag)row.Tag;

            Tx tx = accountService.GetTx(txTag.Id);

            var isMyTX = accountService.IsMyTx(tx.TeammateId);
            var gridView = isMyTX ? _myTxGridView : _cosignTxGridView;

            row.Cells[ColStatus].Value = accountService.GetTxStatusText(tx, isMyTX);

            Padding newPadding = new Padding(0, 2, 2, 1 + Math.Max(1, (tx.Outputs.Count - 1) * _cellFontHeight));

            if (accountService.CanApproveTx(tx))
            {
                row.Cells[ColApprove] = new DataGridViewButtonCell();
                row.Cells[ColApprove].Tag = txTag;
                row.Cells[ColApprove].Value = "Approve";
                row.Cells[ColApprove].Style.Padding = newPadding;
            }
            else
            {
                row.Cells[ColApprove] = new DataGridViewTextBoxCell
                {
                };
            }

            if (accountService.CanBlockTx(tx) || accountService.CanUnblockTx(tx))
            {
                if (false == row.Cells[ColBlock] is DataGridViewButtonCell)
                {
                    row.Cells[ColBlock] = new DataGridViewButtonCell
                    {
                    };
                }
                row.Cells[ColBlock].Tag = txTag;
                row.Cells[ColBlock].Style.Padding = newPadding;
                if (accountService.CanBlockTx(tx))
                {
                    row.Cells[ColBlock].Value = isMyTX ? "Block" : "Decline";
                }
                else
                {
                    row.Cells[ColBlock].Value = isMyTX ? "Unblock" : "Revise";
                }
            }
            else
            {
                row.Cells[ColBlock] = new DataGridViewTextBoxCell
                {
                };
            }
        }

        private void AddOrUpdateTxInGrid(AccountService accountService, Tx tx)
        {
            bool isMyTx = accountService.IsMyTx(tx.TeammateId);
            var gridView = isMyTx ? _myTxGridView : _cosignTxGridView;

            var txTag = GetFromTx(tx);

            DataGridViewRow row = null;
            foreach (DataGridViewRow r in gridView.Rows)
            {
                if (TxTag.Equals(r.Tag as TxTag, txTag))
                {
                    row = r;
                    break;
                }
            }

            if (row == null)
            {
                row = new DataGridViewRow();
                row.Tag = txTag;
                row.CreateCells(gridView);
                gridView.Rows.Insert(0, row);
                row = gridView.Rows[0];
            }

            row.Cells[ColInitiated].Value = tx.InitiatedTime.ToString();
            row.Cells[ColInitiated].Tag = tx.InitiatedTime;
            if (!isMyTx)
            {
                row.Cells[ColFrom].Value = tx.Teammate.Name.Replace(' ', NBSP);
                row.Cells[ColFrom].Tag = txTag;
            }
            var amounts = "";
            var destinations = "";
            var descriptions = "";
            int subRow = 0;
            row.Cells[ColAmount].Tag = txTag;
            row.Cells[ColTo].Tag = txTag;
            row.Cells[ColDescription].Tag = txTag;
            row.Cells[ColStatus].Tag = txTag;
            foreach (var txOutput in tx.Outputs.OrderByDescending(x => x.AmountBTC))
            {
                if (amounts.Length > 0)
                {
                    amounts += Environment.NewLine;
                    destinations += Environment.NewLine;
                    descriptions += Environment.NewLine;
                }
                amounts += GetSubRowText(tx, row.Cells[ColAmount], subRow);
                destinations += GetSubRowText(tx, row.Cells[ColTo], subRow);
                descriptions += GetSubRowText(tx, row.Cells[ColDescription], subRow);
                subRow++;
            }
            row.Cells[ColAmount].Value = amounts;
            row.Cells[ColTo].Value = destinations;
            row.Cells[ColDescription].Value = descriptions;
            UpdateTxStatus(accountService, row);
        }

        private void UpdateTxLists(AccountService accountService)
        {
            var txs = accountService.GetChanged(_updateTime);
            foreach (var tx in txs)
            {
                AddOrUpdateTxInGrid(accountService, tx);
            }

            _updateTime = DateTime.UtcNow;
        }
    }
}

