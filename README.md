# SharpADComputerIP
对于域内很多主机，传统的ping一行命令只能扫描一个C段，并且时间较慢。

通过ldap读取域内主机，然后将主机名转换成IP，这样可以快速收集域内机器，也可以通过这样收集多个网段。


```
C:\Users\testuser\Desktop>SharpADComputerIP.exe
CurrentDomain   sher10ck.local
==================================
Name                  IP
DC              192.168.43.100
TEST08          192.168.43.110
...
```

Cobalt Strike
```
beacon> execute-assembly e:\SharpADComputerIP.exe
[*] Tasked beacon to run .NET program: SharpADComputerIP.exe
[+] host called home, sent: 112683 bytes
[+] received output:
CurrentDomain	sher10ck.local
==================================
Name                  IP
DC		192.168.75.152
TEST08		192.168.43.110
```
