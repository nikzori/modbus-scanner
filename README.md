# Zori's Modbus Scanner

RU | [EN](README_en.md)

������� Modbus � �������� ���������� ���������� � ������� ������ ������������. �������� ��� ����� ���������, ������������ ����� Modbus RTU. ������� ��� ��������� Gidrolock � ���������.

��������� ����������� ������� ��������� ��� �������� �� ����������. 

Modbus TCP ���� � ����������.

## ������������ � ���������
������������ � ��� `.json` ���� � ��������� ������ ������ ��� ���������� ������� ���������. 
������ `checkEntry` � ������������ ��������� ���������� ����� � ����� ��� ����������� ���������� ������������.

���� � ��������� ��������:

```js
{
	// ��� �������/����������
	"name" : "Gidrolock Standard Wi-Fi RS-485",

	// �������� ����������
	"description" : "Smart valve controller unit with wired and wireless leak sensor support",

	// ��������� Modbus ID ����������, � ���������� �������
	// ���� ��� �����������, �������� � ������ � ��������� � ���� ������������
	"modbusID" : 30

	// ������ ������, ���������� � ����������
	// ������ ������ �������� ��������� ����� ������
	// � ������������ ����� ���������� ���������, 
	// � ����� ������� � ����������� ���� ������ (UTF-8, int � �.�.)
	"entries" : [
		{
			// ��� ������
			"name": "Modbus ID",

			// ��� ������������ ���������:
			// "coil", "discrete", "input", "holding"
			"registerType": "holding",

			// ����� ���������� ��������
			// ������ � 0
			"address": 128,

			// ���������� ������������ ���������
			"length": 1,

			// ��� ������ ��� ��������
			// �������������� ����: bool, uint16, uint32, string
			"dataType": "uint16",

			// ���������� �� ��� �������� ��������
			// ��� `false` ������������ ������ � ������ ���
			"readOnce": true
		}
	],

	// ���������� ��� ���������� ��������: ������ ����������, ������ ��������, etc.
	"checkEntry": {
		"registerType": "input", 
		"address": 200,
		"length": 6,
		"dataType": "string",

		// ��������� �������� ��� ������ ���� ���������
		"expectedValue": "STW485"
	}
}
```


### To-Do
- �������� ����������� ������ � ������ ���������������� �������.
	-- �������� ����������� �������� � ��������� ����� ��� ��������� 
- ��������� Modbus TCP

