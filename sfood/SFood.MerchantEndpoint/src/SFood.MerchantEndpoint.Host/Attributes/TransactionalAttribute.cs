﻿using System;

namespace SFood.MerchantEndpoint.Host.Attributes
{
    /// <summary>
    /// 标记action需要启动事务
    /// </summary>
    public class TransactionalAttribute : Attribute
    {
        public TransactionalAttribute() : base()
        {
        }
    }
}