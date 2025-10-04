use service::service::Service;

pub mod service;

fn main() {
    let mut service = Service::new(String::from("Hello, World!"));
    service.handle();
}
