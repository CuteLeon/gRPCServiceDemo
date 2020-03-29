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

​	消息类型可以嵌套引用。但是引用外部文件的消息类型时，需要使用 import "xxx.proto" 导入外部文件。

#### 枚举

​	可以定义在Message里面，也可以单独定义以复用。

​	Enum 的第一个值的Tag必须从0开始，0也是枚举的默认值。

​	枚举值可以起别名，允许两个枚举值使用同一个值。但需要在Enum里写上 `option allow_alias = true;`,

### rpc