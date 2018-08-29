''' Radial Progressbar
''' Creator: Isuru Bandaranayake
''' Date Created: 16/01/2017

Imports System.Drawing.Text
Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Public Class RadialProgressbar
    Inherits Control

#Region "Declarations"
    Private _BaseColour As Color = Color.FromArgb(52, 73, 94)
    Private _ProgressColour As Color = Color.FromArgb(47, 156, 215)
    Private _Value As Integer = 0
    Private _Maximum As Integer = 100
    Private _StartingAngle As Integer = 270
#End Region

#Region "Properties"
    <Category("Control")>
        Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal V As Integer)
            Select Case V
                Case Is < _Value
                    _Value = V
            End Select
            _Maximum = V
            Invalidate()
        End Set
    End Property

    <Category("Control")>
    Public Property Value() As Integer
        Get
            Select Case _Value
                Case 0
                    Return 0
                Case Else
                    Return _Value
            End Select
        End Get

        Set(ByVal V As Integer)
            Select Case V
                Case Is > _Maximum
                    V = _Maximum
                    Invalidate()
            End Select
            _Value = V
            Invalidate()
        End Set
    End Property

    Public Sub Increment(ByVal Amount As Integer)
        Value += Amount
    End Sub

    <Category("Colours")>
    Public Property ProgressColour As Color
        Get
            Return _ProgressColour
        End Get
        Set(ByVal value As Color)
            _ProgressColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(ByVal value As Color)
            _BaseColour = value
        End Set
    End Property

#End Region

#Region "Draw Control"

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(78, 78)
        BackColor = Color.Transparent
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim _Font As Font = New Font("Segoe UI", Me.Height / 7)
        Dim _xFont As Font = New Font("Segoe UI", Me.Height / 20)
        Dim txtSize As SizeF = e.Graphics.MeasureString(_Value, _Font)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        With G
            .TextRenderingHint = TextRenderingHint.AntiAliasGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)

            .FillEllipse(New SolidBrush(_BaseColour), CInt(Width / 10 - 1), CInt(Height / 10 - 1), Width - (CInt(Width / 5)), Height - (CInt(Height / 5)))
            .DrawArc(New Pen(New SolidBrush(_ProgressColour), Width / 10), CInt(Width / 20), CInt(Height / 20), Width - (CInt(Width / 10)) - 2, Height - (CInt(Height / 10)) - 2, 270, CInt((360 / _Maximum) * _Value))
            .DrawString(_Value, _Font, Brushes.White, New Point(Width / 2, Height / 2 - 1), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            .DrawString("%", _xFont, Brushes.White, New Point((txtSize.Width + Width) / 2, ((Height - txtSize.Height) / 2) + e.Graphics.MeasureString("%", _xFont).Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})

        End With

        MyBase.OnPaint(e)
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

#End Region

End Class
