function [ Pmax, Vmp, Imp] = PanelFunction(u,v,w,x,y,z)
a = [0,u;1,u];
b = [0,v;1,v];
c = [0,w;1,w];
d = [0,x;1,x];
e = [0,y;1,y];
f = [0,z;1,z];
%Pmax = u*v;
%Vmp = Pmax/u;
%Imp = Pmax/v;
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here
%*simulationResults = *%
%options = simset('SimulationMode', 'rapid', 'SrcWorkspace','current');
simulationResults = sim('SinglePanelModel', 'FastRestart', 'on', 'SrcWorkspace','current');
V = simulationResults.get('v');
I = simulationResults.get('i');

n = find (I>=0);
n = n(end);
Vglobal = V(1:n);
Iglobal = I(1:n);
%Pmax = 0;
Pglobal = Vglobal.*Iglobal;
n = find(Pglobal == max(Pglobal));
Pmax = Pglobal(n);
Vmp = Vglobal(n);
Imp = Iglobal(n);
end

