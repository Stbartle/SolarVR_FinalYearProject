function [a, b, c, d, e, f] = PanelFunction(u,v,w,x,y,z)
a = [0,u;1,u]
b = [0,v;1,v]
c = [0,w;1,w]
d = [0,x;1,x]
e = [0,y;1,y]
f = [0,z;1,z]
end