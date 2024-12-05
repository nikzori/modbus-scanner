# Gidrolock Modbus Scanner
������� Modbus �������, ���������� ��� ���������� Gidrolock.

������ "������������" ���������� ������ �� ������ ����� Input Register'�� �� ������ `200` (������ ����������/����� ��� ��������� Gidrolock). 

## ������������ � ���������
������������ � ��� `.json` ���� � ��������� ������ ������ ��� ���������� ������� ���������. ������������ ������� � UTF-8.

���� � ��������� ��������:

```json
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
            "dataType": "utf8"
        }
	]

}
```


## To-Do
1. ��������� `.json` �������� ��� ����� ���������
2. ������������� ��������� ����� ���������
3. ��������� Modbus TCP

