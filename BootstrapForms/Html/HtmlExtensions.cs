﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BootstrapForms.Html
{
    /// <summary>
    /// Represents HTML helpers for bootstrap forms
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Replacer for Html.Partial when dealing with nested models
        /// </summary>
        /// <example>
        /// @Html.PartialPrefixed(m => m.Client.Address, "Client/_Address", Model)
        /// </example>
        public static MvcHtmlString BsPartialPrefixed<TModel, TNewModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TNewModel, TProperty>> expression, string partialViewName, TNewModel model,
            string additionalPrefix = "")
        {
            var propertyName = ExpressionHelper.GetExpressionText(expression);
            var backupFieldPrefix = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix;
            var backupModel = htmlHelper.ViewData["Model"];

            var prefix = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix != string.Empty ? "." : string.Empty;
            htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix += prefix + propertyName + additionalPrefix;

            htmlHelper.ViewData["Model"] = model;

            var property = expression.Compile()(model);
            var result = htmlHelper.Partial(partialViewName, property);

            htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = backupFieldPrefix;
            htmlHelper.ViewData["Model"] = backupModel;

            return result;
        }
    }
}