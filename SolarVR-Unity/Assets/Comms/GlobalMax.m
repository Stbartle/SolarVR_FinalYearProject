function [Pmax, Vmp, Imp] = GlobalMax(I, V)
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here

if exist('I', 'var') && exist('V', 'var')
%    figure(1)
%    clf
    n = find (I>=0);
    n = n(end);
    Vglobal = V(1:n);
    Iglobal = I(1:n);
    
    Pglobal = Vglobal.*Iglobal;
    n = find(Pglobal == max(Pglobal));
    if nargout > 2
        Pmax = Pglobal(n);
        Vmp = Vglobal(n);
        Imp = Iglobal(n);
    end
end

