# Gidrolock Modbus Scanner
������� Modbus �������, ���������� ��� ���������� Gidrolock.

������ "������������" ���������� ������ �� ������ ����� Input Register'�� �� ������ `200` (������ ����������/����� ��� ��������� Gidrolock). 

## ������������ � ���������
������������ � ��� `.json` ���� � ��������� ������ ������ ��� ���������� ������� ���������. ������������ ������� � UTF-8.

���� � ��������� ��������:

```js
{
	// ��� �������/����������
	"name" : "Gidrolock Standard Wi-Fi RS-485",

	// �������� ����������
	"description" : "Smart valve controller unit with wired and wireless leak sensor support.",

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
			// �������� ��-���������: 1
			"length": 1,

			// ��� ������ ��� ��������
			// �������������� ����: bool, uint16, uint32, utf8
			// �������� �� ���������: uint16
			"dataType": "uint16",

			// ���������� �� ��� �������� ��������
			// ��� `false` ������������ ������ � ������ ���
			"readOnce": true
		}
	]

}
```


### To-Do
1. ����������� ������ ���������� �� `.json` �������.

	�������� ������� ��������, �� �������� ����� ���������������� ������ ����������. 
```js
	"checkEntry": {
		"registerType": "input"
		"address": 200,
		"length": 6,
		"dataType": "string"
		"expectedValue": "SWT485"
	}
```
2. ������������� ��������� ����� ���������
3. ��������� Modbus TCP

