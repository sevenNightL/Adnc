﻿using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Warehouse.Application.Dtos
{
    public class ProductDto : BaseDto
    {
        public string Id { set; get; }

        public string Sku { set; get; }

        public string Name { set; get; }

        public string Describe { set; get; }

        public float Price { set; get; }

        public ProductStatus ProductStatus { get; set; }

        public string ChangeStatusReason { get; set; }

        public long AssignedWarehouseId { set; get; }

        public string Unit { set; get; }

    }

    public class ProductStatus
    {
        public int Status { get; set; }

        public string StatusDescription { get; set; }

        public string ChangeStatusReason { get; set; }
    }
}
