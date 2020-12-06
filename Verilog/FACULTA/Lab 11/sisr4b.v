module sisr4b(
    input clk,
    input rst_b,
    input i,
    output reg[3:0] q
)
    d_ff dflipflop0(.clk(clk), .rst_b(rst_b), .set_b(1), .d(i ^ q[3]), .q(q[0]));
    d_ff dflipflop1(.clk(clk), .rst_b(rst_b), .set_b(1), .d(q[0] ^ q[3]), .q(q[1]));
    d_ff dflipflop2(.clk(clk), .rst_b(rst_b), .set_b(1), .d(q[1]), .q(q[2]));
    d_ff dflipflop3(.clk(clk), .rst_b(rst_b), .set_b(1), .d(q[2]), .q(q[3]));
endmodule