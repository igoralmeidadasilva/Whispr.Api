using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Core.Extensions;

public static class ColorsExtensions
{
    public static string ToAlertCss(this Colors color)
    {
        return color switch
        {
            Colors.None => string.Empty,
            Colors.Primary => "alert-primary",
            Colors.Secondary => "alert-secondary",
            Colors.Success => "alert-success",
            Colors.Danger => "alert-danger",
            Colors.Warning => "alert-warning",
            Colors.Info => "alert-info",
            Colors.Light => "alert-light",
            Colors.Dark => "alert-dark",
            _ => "alert-primary"
        };
    }

    public static string ToTextCss(this Colors color)
    {
        return color switch
        {
            Colors.None => string.Empty,
            Colors.Primary => "text-primary",
            Colors.Secondary => "text-secondary",
            Colors.Success => "text-success",
            Colors.Danger => "text-danger",
            Colors.Warning => "text-warning",
            Colors.Info => "text-info",
            Colors.Light => "text-light",
            Colors.Dark => "text-dark",
            _ => "alert-primary"
        };
    }

    public static string ToBgCss(this Colors color)
    {
        return color switch
        {
            Colors.None => string.Empty,
            Colors.Primary => "bg-primary",
            Colors.Secondary => "bg-secondary",
            Colors.Success => "bg-success",
            Colors.Danger => "bg-danger",
            Colors.Warning => "bg-warning",
            Colors.Info => "bg-info",
            Colors.Light => "bg-light",
            Colors.Dark => "bg-dark",
            _ => string.Empty
        };
    }

    public static string ToBtnCss(this Colors color)
    {
        return color switch
        {
            Colors.None => string.Empty,
            Colors.Primary => "btn-primary",
            Colors.Secondary => "btn-secondary",
            Colors.Success => "btn-success",
            Colors.Danger => "btn-danger",
            Colors.Warning => "btn-warning",
            Colors.Info => "btn-info",
            Colors.Light => "btn-light",
            Colors.Dark => "btn-dark",
            _ => string.Empty
        };
    }

    public static string ToBgTextCss(this Colors color)
    {
        return color switch
        {
            Colors.None => string.Empty,
            Colors.Primary => "text-bg-primary",
            Colors.Secondary => "text-bg-secondary",
            Colors.Success => "text-bg-success",
            Colors.Danger => "text-bg-danger",
            Colors.Warning => "text-bg-warning",
            Colors.Info => "text-bg-info",
            Colors.Light => "text-bg-light",
            Colors.Dark => "text-bg-dark",
            _ => string.Empty
        };
    }

    public static string ToTextColor(this Colors color)
    {
        if (color is Colors.None or Colors.Light)
        {
            return string.Empty;
        }
        return "text-white";
    }

    public static string ToBtnCloseColor(this Colors color)
    {
        if (color is Colors.None or Colors.Light)
        {
            return string.Empty;
        }
        return "btn-close-white";
    }
}