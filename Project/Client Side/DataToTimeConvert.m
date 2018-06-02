function [a,b,c,d,e,f] = DataToTimeConvert(z,x,v,n,m,l)
%UNTITLED3 Summary of this function goes here
%   Detailed explanation goes here
a = [0,z;1,z];
b = [0,x;1,x];
c = [0,v;1,v];
d = [0,n;1,n];
e = [0,m;1,m];
f = [0,l;1,l];
end

