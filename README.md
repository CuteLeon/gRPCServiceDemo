# GrpcServiceDemo

# Protocol Buffers

​	和开发语言无关；

​	可以生成主流开发语言的代码

​	数据是二进制格式的，串行化效率高，Payload较小

​	适合传输大量数据

## 语法

### syntax

​	声明语法版本。


```
syntax = "proto3";
```

### message

​	声明消息类型。

```
message Person{
	int32 id = 1;
	string name = 2;
	float height = 3;
	float weight = 4;
	Gender gender = 5;
	// repeated 可重复的字段
	repeated string phone_numbers = 6;
	/* 使用 reserved 禁用 Tag 或字段名 
	 */
	reserved 10, 20 to 100, 200 to max;
	reserved "OICQ";
}

enum Gender{
	option allow_alias = true;
	NOT_SPECIFIED = 0;
	WOMAN = 1;
	FEMALE = 1;
	MAN = 2;
	MALE = 2;
}
```

​	数值型、布尔型、字符串（UTF-8或ASCII编码，长度不可超过232）、字节型（长度不可超过232）；

​	字段后面的数字叫做Tag，Tag才是消息串行化时最重要的标识。Tag数值介于 1 ~ 2^29-1，但其中 19000~19999 被保留。

​	Tag 在 1~15 只占用1个字节，所以应该用于频繁使用的字段。Tag 在 16~2047 占用两个字节。

​	在传值的时候，字段可以传或者不传。

​	字段可以重复，使用 repeated 关键字修饰即可。

​	为了消息版本的兼容，被删除的字段应该回收Tag，并标记为 reserved。

### import

```
import "xxx.proto";
```

​	消息类型可以嵌套引用也可以嵌套定义。但是嵌套引用外部文件的消息类型时，需要使用 import "xxx.proto" 导入外部文件。

### package

​	设置命名空间；

```
// 在C#中将默认生成 "My.Project"
package my.project;
// 也可以直接指定特定语言的命名空间
option csharp_namespace = "My.WebAPIs";
```

### 枚举

​	可以定义在Message里面，也可以单独定义以复用。

​	Enum 的第一个值的Tag必须从0开始，0也是枚举的默认值。

​	枚举值可以起别名，允许两个枚举值使用同一个值。但需要在Enum里写上 `option allow_alias = true;`,

### service

​	声明服务。

```
service Employee{
	rpc GetByName (Request) returns (Response)
}
```



### rpc

​	在服务内声明接口方法。

## 消息演进

### 消息演进规则

​	不要修改现有字段的 Tag；

​	新增新的字段，旧的服务会忽略新的字段，因此应该为新字段设置默认值，这个默认值应该是在当前业务没有意义的数据，以免发生混淆；

​	需要删除字段时，被删除的 Tag 应该被回收，永不再用。可以在字段名称加前缀“OBSOLETE”或者使用reserved回收字段；

​	也不应修改字段的类型；

# gRPC原理

## 结构

​	客户端 <=> 生成的代码 <=> 传输协议(Protocol Buffers) <=> 生成的代码 <=> 服务端

## 步骤

​	定义消息 => 生成代码 => 开发客户端和服务端

## 生命周期

​	创建隧道 => 创建Client => Client发送请求 => Server发送 => 发送或接收消息

## 客户端和服务端身份认证（非用户）

​	四种身份认证机制：

1. ​	不安全的连接
2. ​	TLS/SSL
3. ​	基于 Google Token 的身份认证
4. ​	自定义的身份认证提供商

## 消息传输类型

### 一元消息

​	简单的请求-响应，一呼一应。

```protobuf
rpc 方法名(请求message类型) returns(响应message类型)
```

### server streaming

​	服务端会使用流分块传输结果。例如在线观看视频。

```
rpc 方法名(请求message类型) returns(stream 响应message类型)
```

### client streaming

​	客户端分块发送数据，服务端会一直等待，知道所有数据发送完毕。例如上传文件。

```
rpc 方法名(stream 请求message类型) returns(响应message类型)
```

### 双向 streaming

​	综合以上两种。

```
rpc 方法名(stream 请求message类型) returns(stream 响应message类型)
```

