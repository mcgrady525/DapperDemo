USE DapperDemo2DB;
GO

--创建订单表
IF OBJECT_ID('dbo.t_flt_order') IS NOT NULL DROP TABLE dbo.t_flt_order;

CREATE TABLE t_flt_order
(
    id BIGINT NOT NULL IDENTITY,
    created_time DATETIME NULL,
    order_no VARCHAR(32) NULL,
    [status] VARCHAR(32) NULL,
    payment_amt DECIMAL(18,2) NULL,    
    CONSTRAINT pk_t_flt_order PRIMARY KEY(id)
);

--创建订单乘机人表
IF OBJECT_ID('dbo.t_flt_order_passenger') IS NOT NULL DROP TABLE dbo.t_flt_order_passenger;

CREATE TABLE t_flt_order_passenger
(
    id BIGINT NOT NULL IDENTITY,
    created_time DATETIME NULL,
    order_id BIGINT NULL,
    passenger_name NVARCHAR(32) NULL,
    passenger_gender VARCHAR(2) NULL,
    CONSTRAINT pk_t_flt_order_passenger PRIMARY KEY(id)
);

--创建存储过程
--依据orderno获取订单
IF OBJECT_ID('usp_GetFltOrderByOrderNo','P') IS NOT NULL DROP PROC usp_GetFltOrderByOrderNo;
GO

CREATE PROC usp_GetFltOrderByOrderNo
    @OrderNo AS VARCHAR(32)
AS
BEGIN
    SELECT fltOrder.order_no AS OrderNo, fltOrder.payment_amt AS PaymentAmt,fltOrder.created_time AS CreatedTime,* FROM dbo.t_flt_order(NOLOCK) AS fltOrder WHERE fltOrder.order_no= @OrderNo;
END
GO